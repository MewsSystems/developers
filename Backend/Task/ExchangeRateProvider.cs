using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        #region Fields


        private const string URL = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        private const string BASE_CURRENCY_CODE = "CZK";

        private readonly Currency baseCurrency;
        private readonly CultureInfo cultureInfo;


        #endregion

        #region Constructors


        /// <summary>
        /// ExchangeRateProvide constructor.
        /// </summary>
        public ExchangeRateProvider()
        {
            baseCurrency = new Currency(BASE_CURRENCY_CODE);
            cultureInfo = new CultureInfo("cs-CZ");
        }


        #endregion

        #region Public methods


        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            // Currencies not containing base currency is a mistake from the caller and should not raise and exception, just return empty list.
            if (!currencies.Select(c => c.Code).Contains(BASE_CURRENCY_CODE))
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            using (HttpClient client = new HttpClient())
            using (var reader = new StreamReader(await client.GetStreamAsync(URL)))
            {
                // first line contains date of the last update
                await reader.ReadLineAsync();
                // second line contains format for the currencies rates
                await reader.ReadLineAsync();

                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    // lines in format: country|currency|amount|code|rate
                    var tokens = line.Split('|');

                    string currency = tokens[3];
                    if (!currencies.Select(c => c.Code).Contains(currency))
                    {
                        continue;
                    }

                    bool parsedRate = decimal.TryParse(tokens[4], NumberStyles.Currency, cultureInfo, out decimal rate);
                    bool parsedAmount = decimal.TryParse(tokens[2], NumberStyles.Currency, cultureInfo, out decimal amount);

                    if (!parsedRate)
                    {
                        throw new Exception("Failed to parse currency rate.");
                    }
                    if (!parsedAmount)
                    {
                        throw new Exception("Failed to parse currency amount.");
                    }

                    exchangeRates.Add(new ExchangeRate(new Currency(currency), baseCurrency, rate / amount));
                }
            }

            return exchangeRates;
        }

        #endregion
    }
}
