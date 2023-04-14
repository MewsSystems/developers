using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Cached
{
    public class CachedCzechExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly IClock _clock;
        private readonly IMemoryCache _cache;

        public CachedCzechExchangeRateProvider(IExchangeRateProvider exchangeRateProvider, IClock clock, IMemoryCache cache)
        {
            _exchangeRateProvider = exchangeRateProvider;
            _clock = clock;
            _cache = cache;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var cacheKey = $"ExchangeRates_{_clock.Today.ToString(Constants.DateFormat)}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ExchangeRate> exchangeRates))
            {
                exchangeRates = await _exchangeRateProvider.GetExchangeRates(currencies);

                // Store the exchange rates in the cache
                _cache.Set(cacheKey, exchangeRates, TimeSpan.FromDays(1));
            }

            return exchangeRates;
        }
    }
}
