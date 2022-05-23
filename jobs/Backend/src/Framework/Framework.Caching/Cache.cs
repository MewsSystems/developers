using Framework.Caching.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace Framework.Caching
{
	public class Cache : ICache
	{
		private readonly IMemoryCache _memoryCache;

		public Cache(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public T? Get<T>(string key)
		{
			return _memoryCache.TryGetValue(key, out T? value) ? value : default;
		}

		public bool Set<T>(string key, T? value, int ttl = 0)
		{
			if (ttl == 0)
			{
				_memoryCache.Set(key, value);
			}
			else
			{
				_memoryCache.Set(key, value, TimeSpan.FromSeconds(ttl));
			}

			return true;
		}
	}
}
