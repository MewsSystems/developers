using ExchangeRate.Core.Configuration;
using ExchangeRate.Core.Constants;
using ExchangeRate.Core.ExchangeRateSourceClients;
using ExchangeRate.Core.Models;
using ExchangeRate.Core.Models.ClientResponses;
using ExchangeRate.Core.Providers;
using ExchangeRate.Core.Providers.Interfaces;
using ExchangeRate.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExchangeRate.Core;

public class ExchangeRateProvidersFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ExchangeRateProvidersFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, string providerCode)
    {
        IExchangeRateProvider exchangeRateProvider;
        var cacheService = _serviceProvider.GetRequiredService<ICacheService>();

        switch (providerCode)
        {
            case ExchangeRateSourceCodes.CzechNationalBank:
                {
                    var client = _serviceProvider.GetRequiredService<IExchangeRateSourceClient<CnbExchangeRate>>();
                    var cnbSettings = _serviceProvider.GetRequiredService<IOptions<CnbSettings>>().Value;
                    exchangeRateProvider = new CnbExchangeRateProvider(client, cacheService, cnbSettings);
                    break;
                }
            default:
                throw new ArgumentException($"Could not find provider for source code {providerCode}", nameof(providerCode));
        }

        return await exchangeRateProvider.GetExchangeRatesAsync(currencies);
    }
}
