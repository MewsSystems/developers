using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string CnbDailyUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
        private static readonly Currency Czk = new Currency("CZK");
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (currencies == null) return Enumerable.Empty<ExchangeRate>();

            string xml;
            try
            {
                using var httpClient = new HttpClient();
                xml = await httpClient.GetStringAsync(CnbDailyUrl);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error: {ex.Message}");
                return Enumerable.Empty<ExchangeRate>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generic error: {ex.Message}");
                return Enumerable.Empty<ExchangeRate>();
            }

            var requestedCodes = currencies.Select(c => c.Code);

            var document = XDocument.Parse(xml);

            var exchangeRates = document
                .Descendants("radek")
                .Where(row =>
                    row.Attribute("kod") is XAttribute codeAttr &&
                    requestedCodes.Contains(codeAttr.Value))
                .Select(row =>
                {
                    var code = row.Attribute("kod")?.Value;
                    var amountStr = row.Attribute("mnozstvi")?.Value ?? "1";
                    var rateStr = row.Attribute("kurz")?.Value ?? "0";

                    var amount = int.Parse(amountStr, CultureInfo.InvariantCulture);
                    var rate = decimal.Parse(rateStr, new CultureInfo("cs-CZ"));

                    return new ExchangeRate(
                        sourceCurrency: Czk,
                        targetCurrency: new Currency(code),
                        value: rate / amount
                    );
                });

            return exchangeRates;
        }
    }
}