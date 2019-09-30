using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private IExchangeRateParser _parser;

        public ExchangeRateProvider(IExchangeRateParser parser)
        {
            // Throw exception when null parser is provided
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
            {
                yield break;
            }

            // Get rates from specified parser
            var rates = _parser.Parse();
            
            // Convert to list of strings for easier check
            var currencyCodes = currencies.Select(c => c.Code).ToList();

            foreach (ExchangeRate rate in rates)
            {
                // Check both source and target currencies are in list of requested currencies
                if (currencyCodes.Contains(rate.SourceCurrency.Code) && currencyCodes.Contains(rate.TargetCurrency.Code))
                {
                    yield return rate;
                }
            }
        }

    };

}

