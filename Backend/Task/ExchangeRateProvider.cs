using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        public static readonly CultureInfo CzechCultureInfo = CultureInfo.GetCultureInfo("cs-CZ");
        private readonly CnbRateClient _rateClient;

        public ExchangeRateProvider(CnbRateClient client)
        {
            _rateClient = client;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return GetExchangeRatesFromSource(currencies);
        }

        private IEnumerable<ExchangeRate> GetExchangeRatesFromSource(IEnumerable<Currency> validCurrencies)
        {
            string ratesResult = _rateClient.GetRatesDataFromSource();

            return ParseRatesFromSource(ratesResult, validCurrencies);
        }        

        private IEnumerable<ExchangeRate> ParseRatesFromSource(string sourceContents, IEnumerable<Currency> validCurrencies)
        {
            var exchangeRates = new List<ExchangeRate>();

            var rateLines = SplitExchangeRates(sourceContents);

            foreach (var rateLine in rateLines)
            {
                var columns = rateLine.Split('|');

                if (columns.Length < 5)
                {
                    Console.WriteLine("Skipped row containing unexpected data");
                    continue;
                }

                var sourceCurrencyCode = columns[3];

                if (!validCurrencies.Contains(new Currency(sourceCurrencyCode)))
                {
                    continue;
                }

                if (!decimal.TryParse(columns[4], NumberStyles.Currency, CzechCultureInfo, out decimal exchangeRate))
                {
                    Console.WriteLine($"exchange rate for {sourceCurrencyCode} was in an unexpected format");
                    continue;
                }

                exchangeRates.Add(
                    new ExchangeRate(
                        new Currency(sourceCurrencyCode),
                        new Currency("CZK"),
                        exchangeRate));
            }

            return exchangeRates;

        }

        private IEnumerable<string> SplitExchangeRates(string sourceContents)
        {
            return sourceContents
                .Trim()
                .Split('\n')
                .Skip(2);
        }
    }
}
