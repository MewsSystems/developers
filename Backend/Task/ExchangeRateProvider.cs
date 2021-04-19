using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// 
        private readonly IHttpClientWrapper HttpClient;


        const string BANK_URL = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
        const string BASE_CURRENCY = "CZK";

        public ExchangeRateProvider(IHttpClientWrapper httpClient)
        {
            this.HttpClient = httpClient;
        }


        //TODO: Unit tests
        public IEnumerable<ExchangeRateResult> GetExchangeRates(IEnumerable<Currency> currencies)
        {

            List<ExchangeRateResult> result = new List<ExchangeRateResult>();

            using (var client = this.HttpClient)
            {              
                HttpResponseMessage response = client.GetAsync(BANK_URL).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {   
                    var currencyRates = ParseResponse(response.Content.ReadAsStringAsync().Result);

                    result = CreateExchangeRates(currencies, currencyRates);
                }

            }

            return result;
        }

        private List<ExchangeRateResult> CreateExchangeRates(IEnumerable<Currency> currencies, List<CNBCurrencyRate> currencyRates)
        {
            List<ExchangeRateResult> result = new List<ExchangeRateResult>();
            foreach (var currency in currencies)
            {
                var rate = currencyRates.Where(c => c.Code == currency.Code).FirstOrDefault();

                if (rate != null)
                {
                    result.Add(new ExchangeRateResult(true, new ExchangeRate(new Currency(BASE_CURRENCY), new Currency(currency.Code), rate.Value), string.Empty));
                }
                else
                {
                    result.Add(new ExchangeRateResult(false, null, $"Currency with given code {currency.Code} not exist in the Exchange rates from the bank"));
                }
            }

            return result;
        }

        private List<CNBCurrencyRate> ParseResponse(string response)
        {
            List<CNBCurrencyRate> currencyRates = new List<CNBCurrencyRate>();

            XDocument xdoc = XDocument.Parse(response);
            foreach (var child in xdoc.Root.Element("tabulka").Elements())
            {
                currencyRates.Add(new CNBCurrencyRate() { Code = child.Attribute("kod").Value, Value = Convert.ToDecimal(child.Attribute("kurz").Value , new NumberFormatInfo() { NumberDecimalSeparator = "," }) / Convert.ToInt32(child.Attribute("mnozstvi").Value) });
                string code = child.Attribute("kod").Value;
                Console.WriteLine(code);
            }

            return currencyRates;
        }
    }
}
