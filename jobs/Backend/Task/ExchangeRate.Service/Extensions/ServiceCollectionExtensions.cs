using ExchangeRate.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate.Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRateServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IExchangeRateService, ExchangeRateService>();

        return serviceCollection;
    }
}