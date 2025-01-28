using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRateProvider(this IServiceCollection collection)
    {
        collection.AddTransient<ExchangeRateProvider>();
        return collection;
    }
}