using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Cached
{
    // This class provides a caching mechanism for the CnbExchangeRateProvider,
    // so that the exchange rates are fetched only once per day.
    public class CachedCnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly IClock _clock;
        private readonly IMemoryCache _cache;

        public CachedCnbExchangeRateProvider(IExchangeRateProvider exchangeRateProvider, IClock clock, IMemoryCache cache)
        {
            _exchangeRateProvider = exchangeRateProvider;
            _clock = clock;
            _cache = cache;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
        {
            // Use the current date as part of the cache key to ensure
            // a fresh set of exchange rates is fetched each day.
            var cacheKey = $"ExchangeRates_{_clock.Today.ToString(Constants.DateFormat)}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ExchangeRate> exchangeRates))
            {
                exchangeRates = await _exchangeRateProvider.GetExchangeRates(currencies, cancellationToken);

                // Store the exchange rates in the cache
                _cache.Set(cacheKey, exchangeRates, TimeSpan.FromDays(1));
            }

            return exchangeRates;
        }
    }
}
