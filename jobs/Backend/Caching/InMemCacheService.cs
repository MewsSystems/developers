namespace Caching
{
    using System.Runtime.Caching;

    public class InMemCacheService : ICacheService<string, string>
    {
        private readonly ObjectCache _cache = MemoryCache.Default;

        public void Add(string key, string value, DateTimeOffset expire)
        {
            _cache.Add(key, value, new CacheItemPolicy { AbsoluteExpiration = expire });
        }

        public void Clear()
        {
            foreach (var item in _cache)
            {
                this.Remove(item.Key);
            }
        }

        public string Get(string key)
        {
            var value = String.Empty;
            if (_cache.Contains(key))
            {
                value = _cache.Get(key).ToString();
            }

            return value;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
