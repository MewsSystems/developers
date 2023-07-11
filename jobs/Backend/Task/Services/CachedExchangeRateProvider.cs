using ExchangeRateUpdater.Models.Errors;
using ExchangeRateUpdater.Models.Types;
using Microsoft.Extensions.Caching.Memory;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    /// <summary>
    /// Provides an implementation of the <see cref="IExchangeRateProvider"/> interface by caching exchange rates.
    /// </summary>
    internal class CachedExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ExchangeRateProvider _exchangeRateProvider;
        private readonly IMemoryCache _memoryCache;

        private readonly string cacheKey = "exchangeRates";

        public CachedExchangeRateProvider(ExchangeRateProvider exchangeRateProvider, IMemoryCache memoryCache)
        {
            _exchangeRateProvider = exchangeRateProvider;
            _memoryCache = memoryCache;
        }

        public async Task<OneOf<IEnumerable<ExchangeRate>, Error>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if(_memoryCache.TryGetValue(cacheKey, out IEnumerable<ExchangeRate> exchangeRates))
                return exchangeRates.ToList();

            var exchangeRatesResult = await _exchangeRateProvider.GetExchangeRates(currencies);

            exchangeRatesResult.Switch(exchangeRates =>
            {
                CacheExchangeRates(exchangeRates);
            }, Error => {});

            return exchangeRatesResult;
        }

        private void CacheExchangeRates(IEnumerable<ExchangeRate> exchangeRates)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(40))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetPriority(CacheItemPriority.Normal);

            _memoryCache.Set(cacheKey, exchangeRates, cacheEntryOptions);
        }
    }
}
