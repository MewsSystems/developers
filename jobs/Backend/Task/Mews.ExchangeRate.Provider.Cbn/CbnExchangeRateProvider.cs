using Mews.ExchangeRate.Domain;

namespace Mews.ExchangeRate.Provider.Cbn;
public class CbnExchangeRateProvider : IProvideExchangeRates
{
    public Task<IEnumerable<Domain.ExchangeRate>> GetExchangeRatesForCurrenciesAsync(IEnumerable<Currency> currencies)
    {
        return Task.FromResult(Enumerable.Empty<Domain.ExchangeRate>());
    }
}
