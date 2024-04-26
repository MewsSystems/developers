namespace ExchangeRateFinder.Infrastructure.Services
{
    public interface ICachingService<T>
    {
        Task<T> GetOrAddAsync(string key, Func<Task<T>> getItemCallback);
        void UpdateCache(Dictionary<string, T> newCache);
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

        public void UpdateCache(Dictionary<string, T> newCache)
        {
            // Replace the entire cache with the new one
            _cache.Clear();
            foreach (var kvp in newCache)
            {
                _cache[kvp.Key] = kvp.Value;
            }
        }
    }
}
