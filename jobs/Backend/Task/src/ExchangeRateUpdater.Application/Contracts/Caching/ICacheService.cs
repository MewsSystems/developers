namespace ExchangeRateUpdater.Application.Contracts.Caching
{
    public interface ICacheService : IDisposable
    {
        /// <summary>
        /// Adds an element to the cache, with no expiration.
        /// </summary>
        /// <typeparam name="T">Object to caching.</typeparam>
        /// <param name="cacheKey">Key.</param>
        /// <param name="objectToCache">Element to add.</param>
        /// <returns>The element just added.</returns>
        T Add<T>(string cacheKey, T objectToCache);

        /// <summary>
        /// Adds an element to the cache, with expiration in seconds.
        /// </summary>
        /// <typeparam name="T">Object to caching.</typeparam>
        /// <param name="cacheKey">Key.</param>
        /// <param name="objectToCache">Element to add.</param>
        /// <param name="expiration">Expiration in seconds.</param>
        /// <returns>The element just added.</returns>
        T AddWithExpiration<T>(string cacheKey, T objectToCache, long expiration);

        /// <summary>
        /// Check if an element exists in cache.
        /// </summary>
        /// <param name="cacheKey">Element key.</param>
        /// <returns>True if the element exists in the cache. False otherwise.</returns>
        bool Exists(string cacheKey);

        /// <summary>
        /// Gets a cache item. If it doesn't exist in cache, invoke a callback to load it into cache.
        /// </summary>
        /// <typeparam name="T">Object type to caching.</typeparam>
        /// <param name="cacheKey">Element key.</param>
        /// <param name="getItemCallback">Callback.</param>
        /// <returns>The element requested.</returns>
        T Get<T>(string cacheKey, Func<T> getItemCallback);

        /// <summary>
        /// Gets a cache item.
        /// </summary>
        /// <typeparam name="T">Object type to caching.</typeparam>
        /// <param name="cacheKey">Element key.</param>
        /// <returns>The element requested.</returns>
        T Get<T>(string cacheKey);

        /// <summary>
        /// Removes the object associated with the given key.
        /// </summary>
        /// <param name="cacheKey">Element key.</param>
        void Remove(string cacheKey);
    }
}
