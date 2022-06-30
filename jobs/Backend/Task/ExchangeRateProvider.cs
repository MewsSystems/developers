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
        /// Get exchange rates from the CNB.
        /// </summary>
        private static async Task<XmlDocument> LoadXmlData()
        {
            using HttpClient httpClient = new();
            var response = await httpClient.GetStringAsync(baseUrl);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(response);
            return xmlDocument;
        }

        /// <summary>
        /// Parse exchange data from <c>xmlDocument</c> to <c>(code, value)</c> pairs
        /// </summary>
        /// <param name="xmlDocument">Exchange rates from CNB</param>
        private static IEnumerable<(string Code, decimal Rate)> ParseExchangeData(XmlDocument xmlDocument)
        {
            var rows = xmlDocument.GetElementsByTagName("radek");

            foreach (XmlNode row in rows)
            {
                var code = row.Attributes["kod"].Value;
                var quantity = int.Parse(row.Attributes["mnozstvi"].Value);
                var rate = decimal.Parse(row.Attributes["kurz"].Value);
                decimal value = rate / quantity;
                yield return (code, value);
            }
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public static async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<CurrencyPair> currencyPairs)
        {
            var xmlDocumentData = await LoadXmlData();
            var exchangeData =
                ParseExchangeData(xmlDocumentData)
                .Prepend((baseCurrencyCode, 1))
                .ToDictionary(x => x.Code, x => x.Rate);

            var result = currencyPairs.Where(pair =>
                    exchangeData.ContainsKey(pair.SourceCurrency.Code) &&
                    exchangeData.ContainsKey(pair.TargetCurrency.Code))
                .Select(pair =>
                {
                    var (sourceCurrency, targetCurrency) = pair;
                    var value = Math.Round(exchangeData[sourceCurrency.Code] / exchangeData[targetCurrency.Code], 2);
                    return new ExchangeRate(sourceCurrency, targetCurrency, value);
                });
            return result;
        }
    }
}
