using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Abstractions.Model;

namespace ExchangeRateUpdater.Abstractions.Interfaces;

/// <summary>
/// Defines a service for retrieving exchange rates.
/// </summary>
public interface IExchangeRatesService
{
    /// <summary>
    /// Gets exchange rates for the specified currency codes.
    /// </summary>
    /// <param name="currencyCodes"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ExchangeRate>> GetRates(IList<string> currencyCodes);
}