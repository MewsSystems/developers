using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger _logger = null;
        private readonly IApiFetcher _apiFetcher = null;

        public ExchangeRateProvider(ILogger logger, IApiFetcher apiFetcher)
        {
            _logger = logger;
            _apiFetcher = apiFetcher;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // get exchange rates from cb http
            var response = _apiFetcher.GetExchangeRates();

            if (response == null)
            {
                _logger.LogError("Failed to retrieve exchange rates from CNB API");
                return null;
            }

            return FilterRates(response, currencies);
        }

        public static IEnumerable<ExchangeRate> FilterRates(ApiResponse responseJson, IEnumerable<Currency> currencies)
        {
            var rates = new List<ExchangeRate>();
            var filtered = responseJson.Rates.Where(x => currencies.Any(y => y.Code == x.ISOCode)).ToList();

            foreach (var rate in filtered)
            {
                rates.Add(new ExchangeRate(
                    currencies.FirstOrDefault(x => x.Code == rate.ISOCode),
                    currencies.FirstOrDefault(x => x.Code == "CZK"),
                    rate.Rate
                ));
            }

            return rates;
        }
    }
}