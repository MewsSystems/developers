using System.Collections.Generic;
using System.Threading.Tasks;

using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Providers;

public interface IExchangeRateProvider
{
    /// <summary>
    /// Get exchange rates
    /// </summary>
    /// <param name="currencies">The list of currencies for which to get the exchange rate.</param>
    Task<IReadOnlyCollection<ExchangeRate>> GetExchangeRatesAsync(IReadOnlyCollection<Currency> currencies);
}