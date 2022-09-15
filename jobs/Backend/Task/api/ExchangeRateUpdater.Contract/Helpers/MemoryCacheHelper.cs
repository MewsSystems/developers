using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Contract.Helpers;

public class MemoryCacheHelper
{
  private readonly IMemoryCache? _memoryCache;

  public MemoryCacheHelper(IMemoryCache memoryCache)
  {
    _memoryCache = memoryCache;
  }

  public T? GetFromCache<T>(string cacheKey)
  {
    return _memoryCache != null ? _memoryCache.Get<T>(cacheKey) : default;
  }
  
  public void InsertToCache(string cacheKey, object value)
  {
    _memoryCache?.Set(cacheKey, value, 
      new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromHours(4))
        .SetAbsoluteExpiration(TimeSpan.FromHours(12)));
  }
}