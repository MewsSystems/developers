using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
	public interface ICache<T>
	{
		public void Add(string key, T exchangeRate, DateTime? expiration=null);

		public T Get(string key);

		public bool Exists(string key);

		public bool TryGet(string key, out T exchangeRate);
	}

	public class InMemoryExchangeRateCache : ICache<ExchangeRateDitributor>
	{

		private readonly Dictionary<string, CacheEntry> _cache;

		public InMemoryExchangeRateCache()
		{
			_cache = new();
		}

		public void Add(string key, ExchangeRateDitributor exchangeRate, DateTime? utcExpiration = null)
		{
			CacheEntry entry = new(exchangeRate, utcExpiration);
			if(!_cache.TryAdd(key, entry))
			{
				_cache[key] = entry;
			}
		}

		public bool Exists(string key)
		{
			return _cache.TryGetValue(key, out CacheEntry cacheEntry) && !cacheEntry.IsExpired();
		}

		public ExchangeRateDitributor Get(string key)
		{
			if(_cache.TryGetValue(key, out CacheEntry cacheEntry) && !cacheEntry.IsExpired())
				return cacheEntry.Value;
				
			throw new KeyNotFoundException($"Key {key} not found in cache");
		}

		public bool TryGet(string key, out ExchangeRateDitributor exchangeRate)
		{
			if(_cache.TryGetValue(key, out var cacheEntry) && !cacheEntry.IsExpired())
			{
				exchangeRate = cacheEntry.Value;
				return true;
			}
			
			exchangeRate = default;
			return false;
		}

		private class CacheEntry
		{
			public CacheEntry(ExchangeRateDitributor value, DateTime? utcExpiration)
			{
				Value = value;
				_utcExpiration = utcExpiration;
			}
			
			public ExchangeRateDitributor Value { get; }
			
			private readonly DateTime? _utcExpiration = null;
			
			public bool IsExpired()
			{
				if(_utcExpiration.HasValue)
					return _utcExpiration.Value < DateTime.UtcNow;
				
				return false;
			}
		}
	}
}