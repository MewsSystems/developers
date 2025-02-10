using System;
using System.Linq;

namespace ExchangeRateUpdater.Infrastructure.Caching;

/// <summary>
/// Provides utility methods for generating cache keys.
/// </summary>
public static class CacheKeyGenerator
{
    /// <summary>
    /// Generates a cache key based on the entity type and provided key values.
    /// </summary>
    /// <typeparam name="T">The entity type for which the cache key is generated.</typeparam>
    /// <param name="keys">The variable parts of the key, such as identifiers or attributes.</param>
    /// <returns>A formatted cache key string.</returns>
    /// <exception cref="ArgumentException">Thrown when no keys are provided.</exception>
    public static string Generate<T>(params string[] keys)
    {
        if (keys is null || keys.Length == 0)
        {
            throw new ArgumentException("At least one key must be provided.", nameof(keys));
        }

        var entityName = typeof(T).Name;
        var key = string.Join(":", keys.Where(k => !string.IsNullOrWhiteSpace(k)));

        return $"{entityName}:{key}";
    }
}
