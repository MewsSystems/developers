using ExchangeRate.Core.Models;
using ExchangeRate.Core.Providers.Interfaces;
using ExchangeRate.Core.Services;

namespace ExchangeRate.Core;

public class ExchangeRateProvidersFactory
{
    private readonly IExchangeProviderResolver _exchangeProviderResolver;

    public ExchangeRateProvidersFactory(IExchangeProviderResolver exchangeProviderResolver)
    {
        _exchangeProviderResolver = exchangeProviderResolver;
    }

    public async Task<IEnumerable<Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, string providerCode)
    {
        IExchangeRateProvider exchangeRateProvider = _exchangeProviderResolver.GetProvider(providerCode);

        return await exchangeRateProvider.GetExchangeRatesAsync(currencies);
    }
}
