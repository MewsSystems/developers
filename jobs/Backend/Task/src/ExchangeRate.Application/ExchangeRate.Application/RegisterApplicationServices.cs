using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate.Application;

public static class RegisterApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        
        return services;
    }
}