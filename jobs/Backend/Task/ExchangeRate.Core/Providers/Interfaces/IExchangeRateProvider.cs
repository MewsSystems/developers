using ExchangeRate.Core.Models;

namespace ExchangeRate.Core.Providers.Interfaces;

public interface IExchangeRateProvider
{
    /// <summary>
    /// Rerurns list of avilable exchange rates for provided currencies.
    /// </summary>
    /// <param name="currencies">List of currencies for which to return the list of available exchange rates.</param>
    /// <returns>Returns the collection of <c>ExchangeRate.Contracts.Models.ExchangeRate</c>.</returns>
    Task<IEnumerable<Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
}
