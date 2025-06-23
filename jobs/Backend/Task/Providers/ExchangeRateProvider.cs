using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly string _url;
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly ICustomHttpClient _customHttpClient;
        private readonly ICacheService _cache;

        public ExchangeRateProvider(IOptions<ExchangeRateUpdaterConfiguration> configuration, ILogger<ExchangeRateProvider> logger, ICustomHttpClient httpClient, ICacheService cache)
        {
            _url = configuration.Value.CNBExchangeProviderUrl;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _customHttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, DateTime date, CancellationToken cancellationToken)
        {
            if (currencies == null)
            {
                throw new ArgumentNullException(nameof(currencies), "No currencies provided.");
            }

            var key = date.ToString("yyyy-MM-dd");

            if (_cache.TryGetValue(key, out CNBRates cachedResponse))
            {
                _logger.LogInformation($"Using cached response for date {key}");
                return FilterRates(cachedResponse.ToExchangeRates(), currencies);
            }

            _logger.LogInformation($"Fetching exchange rates from CNB for date {key}");
            var data = await _customHttpClient.GetAsync<CNBRates>($"{_url}?date={key}", cancellationToken);

            if (data is null)
            { 
                return Enumerable.Empty<ExchangeRate>();
            }

            _cache.Set(key, data, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheHelpers.GetExpirationTimeSpan(DateTime.UtcNow)
            });

            var rates = data.ToExchangeRates();
            _logger.LogWarning("No rates returned.");

            return FilterRates(rates, currencies);
        }

        private IEnumerable<ExchangeRate> FilterRates(IEnumerable<ExchangeRate> rates, IEnumerable<Currency> currencies)
        {
            var codes = currencies.Select(c => c.Code).ToHashSet();
            return rates.Where(rate => codes.Contains(rate.TargetCurrency.Code));
        }
    }
}
