using ExchangeRateUpdater.Core.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Core.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection AddCore(IServiceCollection services)
    {
        services.AddTransient<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
        
        return services;
    }
}