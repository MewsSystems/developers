using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Infrastructure.CnbApi;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ExchangeRateUpdater.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddRefitClient<ICnbApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.cnb.cz"));

        services.AddScoped<IExchangeRateProvider, CnbExchangeRateProvider>();

        return services;
    }
}