using ExchangeRateUpdater.Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Infrastructure.Caching
{
    public sealed class MemoryCacheService : ICacheService
    {
        private readonly MemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;
        private readonly int _cacheSizeLimit;
        private readonly int _absoluteExpirationInMinutes;

        public MemoryCacheService(IConfiguration configuration)
        {
            // Size doesn’t have a unit. Instead, we need to set the size amount on each cache entry.
            // In this case, we set the amount to 1 each time with SetSize(1). This means that the cache is limited to 1000 items by default.
            _cache = new MemoryCache(new MemoryCacheOptions()
            {
                SizeLimit = int.TryParse(configuration["MemoryCache:SizeLimit"], out _cacheSizeLimit) ? _cacheSizeLimit : 1000
            });

            var absoluteExpirationInMinutes = int.TryParse(configuration["MemoryCache:AbsoluteExpirationInMinutes"], out _absoluteExpirationInMinutes) ? _absoluteExpirationInMinutes : 30;

            _cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1)
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(absoluteExpirationInMinutes)) // It ensures that the data stored in the cache is destroyed at the end of the specified time
                .SetPriority(CacheItemPriority.Normal); // Determines the order in which data is deleted to free memory
        }

        public T Add<T>(string cacheKey, T objectToCache)
        {
            _cache.Set(cacheKey, objectToCache, _cacheEntryOptions);
            return objectToCache;
        }

        public T AddWithExpiration<T>(string cacheKey, T objectToCache, long expiration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1)
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(expiration));

            _cache.Set(cacheKey, objectToCache, cacheEntryOptions);
            return objectToCache;
        }

        public bool Exists(string cacheKey)
        {
            return _cache.TryGetValue(cacheKey, out _);
        }

        public T Get<T>(string cacheKey)
        {
            _cache.TryGetValue(cacheKey, out T item);
            return item!;
        }

        public T Get<T>(string cacheKey, Func<T> getItemCallback)
        {
            if (!_cache.TryGetValue(cacheKey, out T item))
            {
                item = getItemCallback();
                Add(cacheKey, item);
            }

            return item!;
        }

        public void Remove(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}
