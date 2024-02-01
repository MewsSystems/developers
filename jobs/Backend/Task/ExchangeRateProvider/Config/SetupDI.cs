using ExchangeRateProvider.Implementations;
using ExchangeRateProvider.Implementations.CzechNationalBank;
using ExchangeRateProvider.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateProvider.Config;

public static class SetupDI
{
    public static IServiceCollection AddExchangeRateProvider(this IServiceCollection services)
    {
        services.AddHttpClient<IHttpClient, CHttpClient>();
        services.AddSingleton<ICzechNationalBankApi, CzechNationalBankApi>();
        services.AddKeyedTransient<IExchangeRateProvider, ExchangeRateProviderCzechNationalBank>(Source.CzechNationalBank);
        return services;
    }
}