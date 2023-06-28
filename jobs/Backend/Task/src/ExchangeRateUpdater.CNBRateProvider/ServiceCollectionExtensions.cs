using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.CNBRateProvider;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCnbRateProviderIntegration(this IServiceCollection services) => 
        services
            .AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
}
