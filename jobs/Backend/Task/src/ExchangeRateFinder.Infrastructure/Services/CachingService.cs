namespace ExchangeRateFinder.Infrastructure.Services
{
    public interface ICachingService<T>
    {
        Task<T> GetOrAddAsync(string key, Func<Task<T>> getItemCallback);
        Dictionary<string, T> UpdateCache(Dictionary<string, T> newCache);
    }

    public class CachingService<T> : ICachingService<T> where T : class
    {
        private readonly Dictionary<string, T> _cache = new Dictionary<string, T>();

        public async Task<T> GetOrAddAsync(string key, Func<Task<T>> getItemCallback)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }

            var item = await getItemCallback();
            _cache[key] = item;
            return item;
        }

        public Dictionary<string, T> UpdateCache(Dictionary<string, T> newCache)
        {
            var keysToRemove = new List<string>();

            // Update existing elements and identify keys to remove
            foreach (var kvp in _cache)
            {
                if (newCache.ContainsKey(kvp.Key))
                {
                    _cache[kvp.Key] = newCache[kvp.Key];
                    newCache.Remove(kvp.Key); 
                }
                else
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (var keyToRemove in keysToRemove)
            {
                _cache.Remove(keyToRemove);
            }

            foreach (var kvp in newCache)
            {
                _cache[kvp.Key] = kvp.Value;
            }

            return _cache;
        }
    }
}
