using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRateProvider
{
    /// <summary>
    /// Gets exchange rates for the specified currencies for a specific date
    /// </summary>
    /// <param name="currencies">The currencies to get rates for</param>
    /// <param name="date">The date to get exchange rates for (uses today if None)</param>
    /// <returns>Maybe containing collection of exchange rates</returns>
    Task<Maybe<IReadOnlyCollection<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency> currencies, Maybe<DateTime> date);

    /// <summary>
    /// Gets the name of this provider
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Gets the base currency for this provider
    /// </summary>
    string BaseCurrency { get; }
}
