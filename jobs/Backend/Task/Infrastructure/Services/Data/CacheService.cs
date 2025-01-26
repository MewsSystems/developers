using Domain.Abstractions.Data;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services;

internal sealed class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T? Get<T>(string key)
    {
        if (_memoryCache.TryGetValue(key, out var value))
        {
            return (T?)value;
        }

        return default;
    }

    public void Set<T>(string key, T value, TimeSpan expiration)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        _memoryCache.Set(key, value, cacheEntryOptions);
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }

}
