using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string TODAY_EXCHANGE_RATE_URL = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";

        private const string DEFAULT_SOURCE_EXCHANGE_RATE_CODE = "CZK";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string[] desiredCurrencies = FlattenCurrencyCodes(currencies);

            var exchangeRateFileContent = RetrieveExchangeRatesFileContent();

            var linesInExRateFile = exchangeRateFileContent.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var exchangeRates = new List<ExchangeRate>();

            for (int i = 2; i < linesInExRateFile.Length; i++)
            {
                string line = linesInExRateFile[i];
                var colValues = line.Split('|');
                var rateCode = colValues[3];
                var rate = decimal.Parse(colValues[4].Replace(',', '.'));

                if (desiredCurrencies.Contains(rateCode))
                {
                    Currency sourceCurrency = new Currency(DEFAULT_SOURCE_EXCHANGE_RATE_CODE);
                    Currency targetCurrency = new Currency(rateCode);

                    exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, rate));
                }
            }

            return exchangeRates;
        }

        private string[] FlattenCurrencyCodes(IEnumerable<Currency> currencies)
        {
            var desiredCurrencies = new string[currencies.Count()];

            var i = 0;
            foreach (var currency in currencies)
            {
                desiredCurrencies[i] = currency.Code;
                i++;
            }

            return desiredCurrencies;
        }

        private string RetrieveExchangeRatesFileContent()
        {
            var webClient = new WebClient();
            using (var stream = webClient.OpenRead(TODAY_EXCHANGE_RATE_URL))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
