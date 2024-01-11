using ExchangeRateProvider.Implementations.CzechNationalBank;
using ExchangeRateProvider.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateProvider.Config;

public static class SetupDI
{
    public static IServiceCollection AddExchangeRateProvider(this IServiceCollection services)
    {
        services.AddKeyedSingleton<IExchangeRateProvider, ExchangeRateProviderCzechNationalBank>(Source.CzechNationalBank);
        return services;
    }
}

