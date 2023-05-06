using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Net.Http;
using System;

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
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var doc = GetApi();
            var exchangeRates = Enumerable.Empty<ExchangeRate>();
            Currency sourceCurrency = new Currency("CZK");

            if (doc.Descendants("radek") is not null)
            {
                var currencyList = currencies.Select(c => c.Code).ToList();
                var filtered = doc.Descendants("radek").Where(e => currencyList.Contains(e.Attribute("kod").Value));
                exchangeRates = filtered.Select(e => new ExchangeRate(
                                sourceCurrency, 
                                new Currency(e.Attribute("kod").Value),
                                CalcExchangeRate(e.Attribute("mnozstvi").Value, e.Attribute("kurz").Value)
                                )
                );
                return exchangeRates;
            }
            else
            {
                Console.WriteLine("No exchange rate results returned from the API");
                return Enumerable.Empty<ExchangeRate>();
            }
        }

        private decimal CalcExchangeRate(string amount, string value)
        {
            return decimal.Parse(amount) / decimal.Parse(value.Replace(",", "."));
        }

        private static XDocument GetApi()
        {
            try
            {
                string url = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
                XDocument doc = XDocument.Load(url);
                return doc;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("An error occurred while making the HTTP request: {0}", ex.Message);
                return new XDocument();
            }
        }
    }
}
