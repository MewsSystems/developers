using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Tests;

public class TestCache : IMemoryCache
{
    private readonly MemoryCache _cache;

    public TestCache()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    public void Dispose()
    {
        _cache.Dispose();
    }

    public bool TryGetValue(object key, out object? value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public ICacheEntry CreateEntry(object key)
    {
        return _cache.CreateEntry(key);
    }

    public void Remove(object key)
    {
        _cache.Remove(key);
    }
}
