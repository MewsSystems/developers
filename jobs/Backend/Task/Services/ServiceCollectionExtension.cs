using Microsoft.Extensions.DependencyInjection;

namespace Services;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IExchangeRateProvider, CzechCrownRateProvider>();
        services.AddScoped<ICzechNationalBankClient, CzechNationalBankClient>();
        return services;
    }
}