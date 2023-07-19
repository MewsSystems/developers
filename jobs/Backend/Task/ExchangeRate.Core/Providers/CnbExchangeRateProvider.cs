using ExchangeRate.Core.ExchangeRateSourceClients;
using ExchangeRate.Core.Providers.Interfaces;
using ExchangeRate.Core.Models;
using ExchangeRate.Core.Models.ClientResponses;

namespace ExchangeRate.Core.Providers;

public class CnbExchangeRateProvider : CachedExchangeRateProviderBase, IExchangeRateProvider
{
    private readonly IExchangeRateSourceClient<CnbExchangeRateResponse> _cnbExchangeRateClient;

    public CnbExchangeRateProvider(IExchangeRateSourceClient<CnbExchangeRateResponse> cnbExchangeRateClient)
    {
        _cnbExchangeRateClient = cnbExchangeRateClient;
    }

    /// <summary>
    /// Rerurns list of avilable exchange rates for provided currencies based on the Czech National Bank (CNB) sources.
    /// </summary>
    /// <param name="currencies">List of currencies for which to return the list of available exchange rates.</param>
    /// <returns>Returns the collection of <c>ExchangeRate.Contracts.Models.ExchangeRate</c>.</returns>
    public async Task<IEnumerable<Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        return Enumerable.Empty<Models.ExchangeRate>();
    }
}
