using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRateCache
{
    Task<Maybe<IReadOnlyList<ExchangeRate>>> GetCachedRates(IEnumerable<Currency> currencies, Maybe<DateTime> date);
    Task CacheRates(IReadOnlyCollection<ExchangeRate> rates, TimeSpan cacheExpiry);
}
