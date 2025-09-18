using Exchange.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Exchange.Infrastructure.Caching;

public class InMemoryCacheService(IMemoryCache memoryCache, IOptions<CacheOptions> options) : ICacheService
{
    public Task<T?> GetAsync<T>(string key)
    {
        memoryCache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions();
        cacheEntryOptions.SetAbsoluteExpiration(absoluteExpiration ?? options.Value.DefaultAbsoluteExpiration);

        memoryCache.Set(key, value, cacheEntryOptions);
        return Task.CompletedTask;
    }
}