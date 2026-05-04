using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Abstractions.Model;

namespace ExchangeRateUpdater.Abstractions.Interfaces;

/// <summary>
/// Defines a strategy for fetching exchange rates from an external source.
/// </summary>
public interface IExchangeRatesClientStrategy
{
    /// <summary>
    /// Fetches exchange rates from the external source.
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<CurrencyValue>> GetExchangeRates();
}