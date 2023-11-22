using Mews.ExchangeRateProvider.Domain.Common.Dtos.CNBRates;
using Mews.ExchangeRateProvider.Infrastructure.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace Mews.ExchangeRateProvider.Infrastructure.Caching
{
    public class CNBCacheProvider : ICNBCacheProvider
    {
        private readonly IMemoryCache _cache;

        public CNBCacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        //public IEnumerable<ExchangeRate>? GetFromCache(string key)
        //{
        //    var cachedResponse = _cache.TryGetValue(key, out IEnumerable<ExchangeRate>? cachedValues);
        //    if (cachedResponse)
        //    {
        //        return cachedValues;
        //    }
        //    return null;
        //}
        public T? GetFromCache<T>(string key) where T : class
        {
            var cachedResponse = _cache.TryGetValue(key, out T? cachedValues);
            if (cachedResponse)
            {
                return cachedValues;
            }
            return null;
        }

        public void SetCache<T>(string key, T value, MemoryCacheEntryOptions options) where T : class
        {
            _cache.Set(key, value, options);
        }

        public void ClearCache(string key)
        {
            _cache.Remove(key);
        }
    }
}
