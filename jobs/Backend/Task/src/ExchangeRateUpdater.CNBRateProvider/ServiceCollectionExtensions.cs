using ExchangeRateUpdater.CNBRateProvider.Client;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.CNBRateProvider;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCnbRateProviderIntegration(this IServiceCollection services) =>
        services
            .AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
            .AddHttpClient<ICnbClient, CnbClient>()
            .ConfigureHttpClient((sp, httpClient) =>
            {
                // TODO: move to the config
                httpClient.BaseAddress = new Uri("https://api.cnb.cz");
            })
            .Services
        ;
}
