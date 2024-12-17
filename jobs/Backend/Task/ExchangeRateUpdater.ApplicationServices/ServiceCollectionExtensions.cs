using ExchangeRateUpdater.ApplicationServices.ExchangeRates;
using ExchangeRateUpdater.ApplicationServices.MapperProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.ApplicationServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DefaultMapperProfile));
        services.AddScoped<IExchangeRateService, ExchangeRateService>();

        return services;
    }
}
