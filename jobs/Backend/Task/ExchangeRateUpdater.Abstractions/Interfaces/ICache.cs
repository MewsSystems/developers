using System.Collections.Generic;

namespace ExchangeRateUpdater.Abstractions.Interfaces;

/// <summary>
/// Generic cache interface.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICache<T>
{
    /// <summary>
    /// Checks if the cache is empty.
    /// </summary>
    /// <returns></returns>
    bool IsEmpty();
    
    /// <summary>
    /// Retrieves all values from the cache.
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<T> GetAll();

    /// <summary>
    /// Sets a value in the cache.
    /// </summary>
    void Set(string key, T value);
}

