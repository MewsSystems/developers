using ExchangeRate.Core.Models;

namespace ExchangeRate.Core;

public class ExchangeRateProvidersFactory
{
    public async Task<IEnumerable<Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, string providerCode)
    {
        throw new NotImplementedException();
    }
}
