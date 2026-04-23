using Mews.ExchangeRate.Storage.Abstractions;
using Mews.ExchangeRate.Storage.Abstractions.Models;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Mews.ExchangeRate.Storage.Abstractions.Models.StorageStatus;

namespace Mews.ExchangeRate.Storage.DistributedCache.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static T Get<T>(this IDistributedCache cache, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            string value = cache.GetString(key);

            return string.IsNullOrEmpty(value)
                ? default
                : JsonSerializer.Deserialize<T>(value.ToString());
        }

        /// <summary>
        /// Get a cache value by its key asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">The cache.</param>
        /// <param name="key">  The key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">key</exception>
        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            string value = await cache.GetStringAsync(key);

            return string.IsNullOrEmpty(value)
                ? default
                : JsonSerializer.Deserialize<T>(value.ToString());
        }

        /// <summary>
        /// Get (or set if not exists) a cache value by its key asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">  The cache.</param>
        /// <param name="key">    The key.</param>
        /// <param name="func">   The function.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">key</exception>
        public static async Task<T> GetOrSetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> func, DistributedCacheEntryOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var value = await cache.GetAsync<T>(key);

            if (!Equals(value, default(T)))
            {
                return value;
            }

            value = await func.Invoke();

            if (!Equals(value, default(T)))
            {
                await cache.SetAsync(key, value, options);
            }

            return value;
        }

        /// <summary>
        /// Determines whether the cache is ready to be consumed.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <returns>
        ///   <c>true</c> if [is cache ready] [the specified cache]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCacheReady(this IDistributedCache cache)
        {
            var cacheState = cache.Get<StorageState>(StorageKeyBuilder.StorageStateKey);
            var isCacheReady = cacheState != null && cacheState.Values.All(status => status.LastUpdateStatus == UpdateStatus.Success);
            return isCacheReady;
        }

        /// <summary>
        /// Sets a cache entry asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">  The cache.</param>
        /// <param name="key">    The key.</param>
        /// <param name="value">  The value.</param>
        /// <param name="options">The options.</param>
        /// <exception cref="ArgumentNullException">key</exception>
        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            string cacheValue = JsonSerializer.Serialize(value, typeof(T));

            if (options != null)
            {
                await cache.SetStringAsync(key, cacheValue, options);
            }
            else
            {
                await cache.SetStringAsync(key, cacheValue);
            }
        }
    }
}