using ExchangeRateUpdater.Api.HostedServices;

namespace Microsoft.Extensions.DependencyInjection;

public static class HostedServicesConfiguration
{
    public static IServiceCollection ConfigureHostedServices(this IServiceCollection services)
    {
        return services
            .AddHostedService<CnbExchangeRatesUpdater>();
    }
}