using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml;

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

        private const string _baseUrl = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Tuple<Currency, Currency>> currencies)
        {
            var rates = new List<ExchangeRate>();
            var codes = currencies.ToList().Select(x => x.Item1.Code).Union(currencies.ToList().Select(x => x.Item2.Code)).Distinct();
            var exchangeAndRate = new Dictionary<string, decimal>();

            using (HttpClient httpClient = new HttpClient())
            {
                var str = httpClient.GetStringAsync(_baseUrl).Result;

                XmlDocument document = new XmlDocument();
                document.LoadXml(str);

                XmlNodeList rows = document.GetElementsByTagName("radek");
                
                foreach (XmlElement row in rows)
                {
                    var code = row.GetAttribute("kod");

                    if (codes.Contains(code))
                    {
                        var quantity = int.Parse(row.GetAttribute("mnozstvi"));
                        var rate = decimal.Parse(row.GetAttribute("kurz").Replace(',', '.'));
                        decimal value = rate / quantity;

                        exchangeAndRate.Add(code, value);
                    }
                }

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
