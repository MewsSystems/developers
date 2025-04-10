using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Services
{
    public interface ICacheService
    {
        /// <summary>
        /// Sets the value in the cache with the specified key and options.
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="rates">Value to be saved in cache</param>
        /// <param name="options"></param>
        public void Set(object key, CNBRates rates, MemoryCacheEntryOptions options);

        /// <summary>
        /// Tries to get the value from the cache with the specified key.
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="rates">Value</param>
        /// <returns>True if value is found, false is not.</returns>
        public bool TryGetValue(object key, out CNBRates rates);
    }
}
