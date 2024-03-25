using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateDemo.Infrastructure.Services
{
    public class CacheProvider(IMemoryCache cache) : ICacheService
    {
        public async Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> retrieveDataFunc, TimeSpan? slidingExpiration = null)
        {
            if (!cache.TryGetValue(cacheKey, out T cachedData))
            {
                // Data not in cache, retrieve it
                cachedData = await retrieveDataFunc();

                // Set cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(60)
                };

                // Save data in cache
                cache.Set(cacheKey, cachedData, cacheEntryOptions);
            }

            return cachedData;
        }
    }
}
