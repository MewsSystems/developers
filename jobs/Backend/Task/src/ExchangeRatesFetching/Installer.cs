using ExchangeRatesFetching.CNB;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRatesFetching;

public static class Installer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IExchangeRatesAggregator, ExchangeRatesAggregator>();
        services.AddSingleton<IExchangeRatesProviderFactory, ExchangeRatesProviderFactory>();
        services.AddSingleton<IExchangeRatesProvider, CnbExchangeRatesProvider>();
        return services;
    }
}
