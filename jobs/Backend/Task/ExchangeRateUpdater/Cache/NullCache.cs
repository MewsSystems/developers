using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Cache;

/// <summary>
/// A dummy implementation of cache that is injected into all consumers.
/// </summary>
public class NullCache : IExchangeRateCache
{
    public async Task<IEnumerable<ExchangeRate>> GetOrCreateAsync(string key, Func<Task<IEnumerable<ExchangeRate>>> createFunc, DateTime? expiration = null) => await createFunc();

    public void Remove(string key)
    {
    }

    public static NullCache Instance { get; } = new();
}