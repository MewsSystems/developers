using ExchangeRateUpdater.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure;

public class ExchangeRateCache : IExchangeRateCache
{
    private readonly IMemoryCache _cache;
    private static readonly SemaphoreSlim _lock = new(1, 1);
    private const string ExchangeRates = "ExchangeRates";

    public ExchangeRateCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<ExchangeRate[]> GetCachedExchangeRatesAsync()
    {
        await _lock.WaitAsync();

        try
        {
            if (_cache.TryGetValue(ExchangeRates, out ExchangeRate[] cachedRates))
            {
                return cachedRates;
            }

            return null;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task SetExchangeRatesCacheAsync(ExchangeRate[] exchangeRates)
    {
        await _lock.WaitAsync();

        try
        {
            _cache.Set(ExchangeRates, exchangeRates);
        }
        finally
        {
            _lock.Release();
        }
    }
}
