namespace ExchangeRateUpdater.Worker;

using Serilog;
using Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRateWorkerServices(this IServiceCollection services)
    {
        services.AddSerilog();
        services.AddSingleton<ICzYearProcessor, CzYearProcessor>();
        services.AddSingleton<IPerDayProcessor, PerDayProcessor>();

        return services;
    }
}