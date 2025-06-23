using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace ExchangeRateUpdater.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public void Set(object key, CNBRates rates, MemoryCacheEntryOptions options)
        {
            _cache.Set(key, rates, options);
        }

        public bool TryGetValue(object key, out CNBRates rates)
        {
            return _cache.TryGetValue(key, out rates);
        }
    }
}
