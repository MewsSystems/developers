using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using static ExchangeRateUpdater.Program;

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
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<CurrencyPair> currencies)
        {
            var rates = new List<ExchangeRate>();
            var codes = currencies.Select(x => x.SourceCurrency.Code).Union(currencies.Select(x => x.TargetCurrency.Code)).Distinct();
            var exchangeAndRate = new Dictionary<string, decimal>();

            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(baseUrl);
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

                List<string> usedCodesList = new List<string>(exchangeAndRate.Keys);

                // Round values to 2 decimal places and add them to rates list
                foreach (var currency in currencies)
                {
                    var curr1 = currency.SourceCurrency.Code;
                    var curr2 = currency.TargetCurrency.Code;
                    if (usedCodesList.Contains(curr1) && usedCodesList.Contains(curr2))
                    {
                        var value = Math.Round(exchangeAndRate[curr1] / exchangeAndRate[curr2], 2);
                        rates.Add(new ExchangeRate(currency.SourceCurrency, currency.TargetCurrency, value));
                    }
                }
            }
            return rates;
        }
    }
}
