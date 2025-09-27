using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Extensions;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Console.Services;

public class NoOpExchangeRateCache : IExchangeRateCache
{
    public Task<Maybe<IReadOnlyList<ExchangeRate>>> GetCachedRates(IEnumerable<Currency> currencies, Maybe<DateTime> date)
    {
        return Maybe<IReadOnlyList<ExchangeRate>>.Nothing.AsTask();
    }

    public Task CacheRates(IReadOnlyCollection<ExchangeRate> rates, TimeSpan cacheExpiry)
    {
        return Task.CompletedTask;
    }

    public Task ClearCache()
    {
        return Task.CompletedTask;
    }
}
