using Exchange.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Application.Extensions;

public static class ExchangeRateProviderServiceCollectionExtension
{
    public static IServiceCollection AddExchangeRateProvider(this IServiceCollection services)
    {
        services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
        return services;
    }
}