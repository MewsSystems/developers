using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Infrastructure.Cache;
using ExchangeRateUpdater.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services
            .ConfigureApplicationServices()
            .ConfigureInfrastructureServices();
    }

    private static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IExternalExchangeRatesProvider, ExternalExchangeRatesProvider>();
    }

    private static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICzechNationalBankService, CzechNationalBankService>()
            .AddScoped<ICzechNationalBankCacheAccessor, CzechNationalBankCacheAccessor>();
    }
}
