using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Abstractions.Interfaces;
using ExchangeRateUpdater.Abstractions.Model;

namespace ExchangeRateUpdater.CnbClient.Implementation;

/// <summary>
/// In-memory implementation of a cache for currency values.
/// </summary>
public class InMemoryExchangeRateCache : ICache<CurrencyValue>
{
    private readonly ConcurrentDictionary<string, CurrencyValue> cache = new();

    /// <summary>
    /// Checks if the cache is empty.
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        return !cache.Any();
    }

    /// <summary>
    /// Retrieves all currency values from the cache.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<CurrencyValue> GetAll()
    {
        return cache.Values.ToList();
    }

    /// <summary>
    /// Sets a currency value in the cache.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Set(string key, CurrencyValue value)
    {
        cache[key] = value;
    }
}
