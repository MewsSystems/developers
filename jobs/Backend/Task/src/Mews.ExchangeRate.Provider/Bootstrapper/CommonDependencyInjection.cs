using Microsoft.Extensions.DependencyInjection;
using Mews.ExchangeRate.Storage.DistributedCache.Extensions;
using Mews.ExchangeRate.Http.Cnb.Extensions;
using Mews.ExchangeRate.Updater.Services.Abstractions;
using Mews.ExchangeRate.Updater.Services;
using Mews.ExchangeRate.Provider.Services.Abstractions;
using Mews.ExchangeRate.Updater.Services.Worker;

namespace Mews.ExchangeRate.Provider.Bootstrapper.Bootstrapper;
public static class CommonDependencyInjection
{

    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddExchangeRateHttpServices();
        services.AddExchangeRateStorage();

        services.AddScoped<IClock, SystemClock>();
        services.AddScoped<IExchangeRateUpdateService, ExchangeRateUpdateService>();

        services.AddScoped<IExchangeRateService, ExchangeRateService>();

        // [JUAN] Those hosted services are not needed in a microservice architecture.
        // That is the reason why they belong to the Updater project.
        services.AddHostedService<CurrencyExchangeWorkerService>();
        services.AddHostedService<ForeignCurrencyExchangeWorkerService>();

        return services;
    }
}
