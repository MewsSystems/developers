using ExchangeRateUpdater.Domain.Entities;

namespace Adapter.ExchangeRateProvider.InMemory;

/// <summary>
/// Class representing a cache key.
/// </summary>
internal class CacheKey
{
    /// <summary>
    /// Cache Key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Last time the values was accessed. Used by the simple LRU use case.
    /// </summary>
    public DateTime LastAccessedTime {get; private set;}

    /// <summary>
    /// The expiry date of the cache value.
    /// </summary>
    public DateTime ExpiryDate { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="from">Beginning date of time interval.</param>
    /// <param name="to">End date of time interval</param>
    /// <param name="type">Type of <see cref="CacheType"/></param>
    /// <param name="creationTime">Creation time of cache key.</param>
    /// <param name="ttl">Time to live of a cache key and by reference a cache instance.</param>
    public CacheKey(DateTime from, DateTime to, CacheType type, DateTime creationTime, TimeSpan ttl)
    {
        Key = $"{type}.{from}.{to}";
        LastAccessedTime = creationTime;
        ExpiryDate = creationTime.Add(ttl);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is CacheKey cacheKey && cacheKey.Key == Key;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString() 
    {
        return Key;
    }

   
}
