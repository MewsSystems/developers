using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Extensions;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Infrastructure.Caching;

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
