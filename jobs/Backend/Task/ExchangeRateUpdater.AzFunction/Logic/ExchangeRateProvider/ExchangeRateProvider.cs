using ExchangeRateUpdater.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

namespace ExchangeRateUpdater.AzFunction.Logic.ExchangeRateProvider
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _currency;

        /// <summary>
        /// The thing here is that dotnet provided this new IHttpClientFactory interface recently. 
        /// It automatically manages the HttpClients so you always have one available in case you need it
        /// and it's not required to dispose them.
        /// </summary>
        /// <param name="httpClient"></param>
        public ExchangeRateProvider(IHttpClientFactory httpClient, string currency)
        {
            _httpClient = httpClient.CreateClient();
            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("CNBCZechNationalBankBaseAddress"));
            _currency = currency;

        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies.Count() == 0)
                return Enumerable.Empty<ExchangeRate>();

            // I found that CNB has a XML file with the daily currencies. This is updated every date at 14:30pm. The ideal thing here is to cache the results.
            // As I said on the Function class, Azure APIM is a good servcie for this kind of things.
            var apiResponse = await _httpClient.GetAsync(Environment.GetEnvironmentVariable("CNBCZechNationalBankApi"));

            if (!apiResponse.IsSuccessStatusCode)
            {
                throw new HttpResponseException(HttpStatusCode.Gone);
            }

            var content = await apiResponse.Content.ReadAsStringAsync();
            var doc = XDocument.Parse(content);

            NumberFormatInfo formatInfo = (NumberFormatInfo)NumberFormatInfo.InvariantInfo.Clone();
            formatInfo.NumberDecimalSeparator = ",";
            // Just get the structure of the XML and create the ExchangeRate objects
            var exchangeRates = (from c in doc.Elements("kurzy").Elements("tabulka").Elements()
                                 where currencies.Any(item => item.Code == (string)c.Attribute("kod"))
                                 select new ExchangeRate(new Currency((string)c.Attribute("kod")), new Currency(_currency), decimal.Parse((string)c.Attribute("kurz"), formatInfo))
                                 ).ToList();

            return exchangeRates;
        }
    }
}
