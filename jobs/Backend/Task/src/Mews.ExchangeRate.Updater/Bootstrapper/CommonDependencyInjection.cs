using Mews.ExchangeRate.Updater.Services.Worker;
using Microsoft.Extensions.DependencyInjection;
using Mews.ExchangeRate.Storage.DistributedCache.Extensions;
using Mews.ExchangeRate.Http.Cnb.Extensions;
using Mews.ExchangeRate.Updater.Services.Abstractions;
using Mews.ExchangeRate.Updater.Services;

namespace Mews.ExchangeRate.Updater.Bootstrapper;
public static class CommonDependencyInjection
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {        
        services.AddExchangeRateHttpServices();
        services.AddExchangeRateStorage();

        services.AddScoped<IClock, SystemClock>();
        services.AddScoped<IExchangeRateUpdateService, ExchangeRateUpdateService>();

        services.AddHostedService<CurrencyExchangeWorkerService>();
        services.AddHostedService<ForeignCurrencyExchangeWorkerService>();

        return services;
    }
}
