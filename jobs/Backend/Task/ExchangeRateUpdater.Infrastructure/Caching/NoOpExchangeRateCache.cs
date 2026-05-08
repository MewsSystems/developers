using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Extensions;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Infrastructure.Caching;

public class NoOpExchangeRateCache : IExchangeRateCache
{
    public Task<Maybe<IReadOnlyList<ExchangeRate>>> GetCachedRates(IEnumerable<Currency> currencies, DateOnly date)
    {
        return Maybe<IReadOnlyList<ExchangeRate>>.Nothing.AsTask();
    }

    public Task CacheRates(IReadOnlyCollection<ExchangeRate> rates)
    {
        return Task.CompletedTask;
    }

    public Task ClearCache()
    {
        return Task.CompletedTask;
    }
}
