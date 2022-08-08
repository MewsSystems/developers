using Domain.Entities;
using Domain.Ports;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.InMemoryCache;

public class CacheHelper : IExchangeRateInMemoryCache
{
    private readonly IMemoryCache _memoryCache;
    
    public CacheHelper(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public IEnumerable<ExchangeRate>? GetCache(string key) => _memoryCache.Get<IEnumerable<ExchangeRate>>(key);

    public void SetCache(string key, object exchangeRates)
    {
        var memoryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(3))
            .SetSlidingExpiration(TimeSpan.FromHours(5))
            .SetPriority(CacheItemPriority.Normal);
        
        _memoryCache.Set(key, exchangeRates, memoryOptions);
    }
}