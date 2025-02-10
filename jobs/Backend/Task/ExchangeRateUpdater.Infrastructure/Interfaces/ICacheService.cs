using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Interfaces;

/// <summary>
/// Defines a contract for a caching service that retrieves and stores data in a cache.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Retrieves an item from the cache or fetches and caches it if not found.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="cacheKey">The unique key identifying the cached item.</param>
    /// <param name="fetchFunction">A function that retrieves the data if it is not found in the cache.</param>
    /// <returns>The cached item if found, otherwise the fetched item.</returns>
    Task<T?> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> fetchFunction);
}
