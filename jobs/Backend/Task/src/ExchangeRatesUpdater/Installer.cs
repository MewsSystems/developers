using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRatesUpdater;

public static class Installer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ExchangeRatesCoordinator>();
        services.AddSingleton<ParametersParser>();

        return services;
    }
}
