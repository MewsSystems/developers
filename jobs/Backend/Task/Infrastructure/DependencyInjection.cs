using Application.Abstractions;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddMemoryCache();
        services.AddTransient<CnbApiClient.CnbApiClient>();

        services.AddScoped<IExchangeRatesService, ExchangeRatesService>();

        return services;
    }
}