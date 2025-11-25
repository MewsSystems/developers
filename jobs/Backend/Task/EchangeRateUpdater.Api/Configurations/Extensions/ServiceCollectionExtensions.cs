namespace EchangeRateUpdater.Api.Configurations.Extensions;

using Serilog;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHealthChecks();

        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddSerilog()
            .AddOpenApiConfiguration();

        return services;
    }
}