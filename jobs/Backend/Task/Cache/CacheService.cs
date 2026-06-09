using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Cache
{
    public class CacheService<TKey, TValue>: ICacheService<TKey, TValue>
    {
        private readonly Dictionary<TKey, CacheItem<TValue>> _cache = new Dictionary<TKey, CacheItem<TValue>>();

        private class CacheItem<T>
        {
            public T Value { get; set; }
            public DateTime ExpirationTime { get; set; }
        }

        public void Add(TKey key, TValue value, TimeSpan duration)
        {
            var expirationTime = DateTime.UtcNow + duration;
            var cacheItem = new CacheItem<TValue> { Value = value, ExpirationTime = expirationTime };

            _cache[key] = cacheItem;
        }

        public TValue Get(TKey key)
        {
            if (_cache.TryGetValue(key, out var cacheItem))
            {
                if (cacheItem.ExpirationTime >= DateTime.UtcNow)
                {
                    return cacheItem.Value;
                }
                else
                {
                    _cache.Remove(key);
                }
            }

            return default(TValue);
        }
    }
}
