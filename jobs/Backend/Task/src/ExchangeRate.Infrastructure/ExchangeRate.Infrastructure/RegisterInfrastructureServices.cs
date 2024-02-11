using ExchangeRate.Application.Services;
using ExchangeRate.Infrastructure.Cnb;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate.Infrastructure;

public static class RegisterInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IExchangeRatesService, ExchangeRatesService>();
        services.AddTransient<IExchangeRateFetcher, ExchangeRateFetcher>();
        
        return services;
    }
}