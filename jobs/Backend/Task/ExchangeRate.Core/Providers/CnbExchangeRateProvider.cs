using ExchangeRate.Contracts.Models;
using ExchangeRate.Core.Providers.Interfaces;

namespace ExchangeRate.Core.Providers;

public class CnbExchangeRateProvider : IExchangeRateProvider
{
    /// <summary>
    /// Rerurns list of avilable exchange rates for provided currencies based on the Czech National Bank (CNB) sources.
    /// </summary>
    /// <param name="currencies">List of currencies for which to return the list of available exchange rates.</param>
    /// <returns>Returns the collection of <c>ExchangeRate.Contracts.Models.ExchangeRate</c>.</returns>
    public async Task<IEnumerable<Contracts.Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        return Enumerable.Empty<Contracts.Models.ExchangeRate>();
    }
}
