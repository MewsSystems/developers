using System;
using System.Collections.Generic;
using models.ExchangeRateUpdater;
using System.Linq;
using System.Net.Http;
using System.Xml;
using ExchangeRateUpdater.Models;
using Newtonsoft.Json;

public class ExchangeRateProviderService : IExchangeServiceProviderService
{


    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public List<ExchangeRate> GetExchangeRates(List<CurrencyModel> currencies, Settings settings)
    {
        //Get current rates
        var url = settings.BankRatesUrl;
        var httpClient = new HttpClient();
        var result = httpClient.GetAsync(url).Result;

        //Serialize rates to c# object
        var xmlString = result.Content.ReadAsStringAsync().Result;
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);
        var serializedXml = JsonConvert.SerializeXmlNode(xmlDoc);
        var json = JsonConvert.DeserializeObject<BankRatesModel>(serializedXml, new JsonSerializerSettings
        {
            // fr culture separator is "," as per xml
            Culture = new System.Globalization.CultureInfo("fr-FR")
        });


        var RateList = json.Root.RateList.List;
        var exchangeRates = new List<ExchangeRate>();
        foreach (var rateCurrency in currencies)
        {
            //check if the currency code exist in the serialized data from the bank
            var rate = RateList.Where(x => x.Code == rateCurrency.Code).FirstOrDefault();
            if (rate != null)
            {

                var targetCurrency = new CurrencyModel(rate.Code);
                var sourceCurrency = new CurrencyModel(settings.BaseCurrency);
                var calculatedValue =  rate.RateValue/rate.Value;
                exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, Math.Round(calculatedValue, settings.ExchangePrecision)));

            }
        }

        return exchangeRates;
    }
}