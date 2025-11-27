namespace ExchangeRateUpdater.Worker;

using Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRateWorkerServices(this IServiceCollection services)
    {
        services.AddSingleton<ICzYearProcessor, CzYearProcessor>();
        services.AddSingleton<IPerDayProcessor, PerDayProcessor>();

        return services;
    }
}