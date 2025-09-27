using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRateProvider
{
    /// <summary>
    /// Gets exchange rates for the specified currencies for a specific date
    /// </summary>
    /// <param name="date">The date to get exchange rates for (uses today if None)</param>
    /// <returns>Maybe containing collection of exchange rates</returns>
    Task<Maybe<IReadOnlyCollection<ExchangeRate>>> GetExchangeRatesForDate(Maybe<DateTime> date);

    string ProviderName { get; }
    string BaseCurrency { get; }
}
