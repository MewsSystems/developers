using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Interfaces;

public interface IExchangeRateCache
{
    Task<Maybe<IReadOnlyList<ExchangeRate>>> GetCachedRates(IEnumerable<Currency> currencies, Maybe<DateTime> date);
    Task CacheRates(IReadOnlyCollection<ExchangeRate> rates, TimeSpan cacheExpiry);
}
