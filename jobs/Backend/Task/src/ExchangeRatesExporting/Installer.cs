using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRatesExporting;

public static class Installer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IConsoleExporter, ConsoleExporter>();
        return services;
    }
}
