using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Infrastructure.CnbApi;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ExchangeRateUpdater.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddRefitClient<ICnbApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.cnb.cz"));

        services.AddScoped<IExchangeRateProvider, CnbExchangeRateProvider>();

        return services;
    }
}