using ERU.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ERU.Application;

public class MemoryCache : ICache
{
	private readonly int _absoluteExpirationInMinutes;
	private readonly int _slidingExpirationInMinutes;
	private readonly IMemoryCache? _memoryCache;

	public MemoryCache(IMemoryCache memoryCache, int absoluteExpirationInMinutes, int slidingExpirationInMinutes)
	{
		_memoryCache = memoryCache;
		_absoluteExpirationInMinutes = absoluteExpirationInMinutes;
		_slidingExpirationInMinutes = slidingExpirationInMinutes;
	}

	public T? GetFromCache<T>(string cacheKey)
	{
		return _memoryCache != null ? _memoryCache.Get<T>(cacheKey) : default;
	}

	public void InsertToCache(string cacheKey, object value)
	{
		_memoryCache?.Set(cacheKey, value,
			new MemoryCacheEntryOptions()
				.SetSlidingExpiration(TimeSpan.FromMinutes(_slidingExpirationInMinutes))
				.SetAbsoluteExpiration(TimeSpan.FromMinutes(_absoluteExpirationInMinutes)));
	}
}