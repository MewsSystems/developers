using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Cache;

/// <summary>
/// A dummy implementation of cache that is injected into all consumers by default.
/// </summary>
public class NullCache : IExchangeRateCache
{
    public async Task<IEnumerable<ExchangeRate>> GetOrCreateAsync(string key, Func<Task<IEnumerable<ExchangeRate>>> createFunc, Func<DateTime?> expirationDateCreationFunc) => await createFunc();

    public void Remove(string key)
    {
    }

    public static NullCache Instance { get; } = new();
}