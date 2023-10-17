using Mews.ExchangeRateProvider.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.ExchangeRateProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRateProvider(this IServiceCollection services, IConfiguration configurationSection)
    {
        services
            .Configure<ExchangeRateProviderOptions>(configurationSection)
            .AddSingleton<CzechNationalBankExchangeRateMapper>()
            .AddHttpClient<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();

        return services;
    }
}