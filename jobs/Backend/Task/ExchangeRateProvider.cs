using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string TargetCurrencyCode = "CZK";
        private static readonly Currency TargetCurrency = new(TargetCurrencyCode);

        private readonly IExchangeRatesSource _ratesSource;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProvider(IExchangeRatesSource ratesSource, ILogger<ExchangeRateProvider> logger)
        {
            _ratesSource = ratesSource ?? throw new ArgumentNullException(nameof(ratesSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var requestedCodes = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);

            if (!requestedCodes.Contains(TargetCurrencyCode))
            {
                _logger.LogDebug("CZK not requested; returning empty result set");
                return [];
            }

            // Future improvement: add caching here to avoid repeated HTTP calls for the same day's rates.
            var content = _ratesSource.GetLatestRatesContent();
            var rates = CnbRatesParser.Parse(content, requestedCodes, TargetCurrency).ToList();

            _logger.LogDebug("Returning {Count} rates for requested currencies", rates.Count);

            return rates;
        }
    }
}
