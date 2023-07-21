using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRate.Core.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<T?> GetAsync<T>(string cacheKey)
        {
            var cache = _memoryCache.Get<T>(cacheKey);
            return Task.FromResult(cache);
        }

        public Task<bool> SetAsync<T>(string cacheKey, T value)
        {
            _memoryCache.Set(
                cacheKey,
                value,
                new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.UtcNow.AddDays(1).Date.Subtract(DateTime.UtcNow)));
            return Task.FromResult(true);
        }
    }
}
