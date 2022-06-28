using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string baseCurrencyCode = "CZK";
        private const string baseUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Tuple<Currency, Currency>> currencies)
        {
            var rates = new List<ExchangeRate>();
            var codes = currencies.ToList().Select(x => x.Item1.Code).Union(currencies.ToList().Select(x => x.Item2.Code)).Distinct();
            var exchangeAndRate = new Dictionary<string, decimal>();

            using (HttpClient httpClient = new HttpClient())
            {
                var response = httpClient.GetStringAsync(baseUrl).Result;
                var doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNodeList rows = doc.GetElementsByTagName("radek");

                foreach (XmlNode row in rows)
                {
                    var code = row.Attributes["kod"].Value;
                    if (codes.Contains(code))
                    {
                        var quantity = int.Parse(row.Attributes["mnozstvi"].Value);
                        var rate = decimal.Parse(row.Attributes["kurz"].Value);
                        decimal value = rate / quantity;
                        exchangeAndRate.Add(code, value);
                    }
                }

                // Currency rate of base currency should always equals to 1
                // Add it, if source doesn't provide it
                if (!exchangeAndRate.ContainsKey(baseCurrencyCode))
                {
                    exchangeAndRate.Add(baseCurrencyCode, 1);
                }

                // Round values to 2 decimal places and add them to rates list
                foreach (var currency in currencies)
                {
                    var value = Math.Round(exchangeAndRate[currency.Item1.Code] / exchangeAndRate[currency.Item2.Code], 2);
                    rates.Add(new ExchangeRate(currency.Item1, currency.Item2, value));
                }
            }
            return rates;
        }
    }
}
