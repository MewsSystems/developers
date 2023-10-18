using Mews.ExchangeRateProvider.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.ExchangeRateProvider.Extensions;

/// <summary>
/// DI Container extension methods for supplying exchange rate providers
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the configuration, exchange rate provider, HTTP client and mapper classes for obtaining exchange rates to this DI container
    /// </summary>
    /// <param name="services">The DI Service Collection</param>
    /// <param name="configurationSection">A configuration section which details the URIs to obtain exchange rate data from</param>
    /// <returns>The DI Service Collection</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddExchangeRateProvider(this IServiceCollection services, IConfiguration configurationSection)
    {
        if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection), "You must supply a configuration section for the CzechNationalBankExchangeRateProviderOptions");

        services
            .Configure<CzechNationalBankExchangeRateProviderOptions>(configurationSection)
            .AddSingleton<CzechNationalBankExchangeRateMapper>()
            .AddHttpClient<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();

        return services;
    }
}