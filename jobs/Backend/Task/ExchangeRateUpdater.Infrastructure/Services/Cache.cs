using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Infrastructure.Services
{
    internal class Cache : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public Cache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool Set<T>(string key, T value, DateTimeOffset expireOn)
        {
            var result = _memoryCache.Set(key, value, expireOn);

            return result != null;
        }

        public T? Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }
    }
}
