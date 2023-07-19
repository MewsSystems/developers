using ExchangeRate.Contracts.Models;

namespace ExchangeRate.Core;

public class ExchangeRateProvidersFactory
{
    public async Task<IEnumerable<Contracts.Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, string providerCode)
    {
        throw new NotImplementedException();
    }
}
