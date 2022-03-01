using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Providers.Cnb;

namespace ExchangeRateUpdater.Services;

public class CurrencyRateService : ICurrencyRateService
{
    private readonly IDictionary<string, IExchangeRateProvider> _providers;

    public CurrencyRateService(IHttpClientFactory httpClientFactory)
    {
        var httpClientFactory1 = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        _providers = new Dictionary<string, IExchangeRateProvider>
        {
            { "CNB", new CnbExchangeRateProvider(httpClientFactory1) }
        };
    }
    
    /// <inheritdoc />
    public Task<IEnumerable<ExchangeRate>> GetCurrencyRatesAsync(
        string providerName,
        IEnumerable<Currency> fromCurrencies)
    {
        if (string.IsNullOrEmpty(providerName))
            throw new ArgumentNullException(nameof(providerName));

        providerName = providerName.ToUpperInvariant();

        if (_providers.TryGetValue(providerName, out var provider))
        {
            return provider.GetExchangeRatesAsync(fromCurrencies);
        }

        throw new NutSupportedRateProviderException();
    }
}