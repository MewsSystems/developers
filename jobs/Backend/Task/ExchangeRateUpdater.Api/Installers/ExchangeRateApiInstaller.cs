using ExchangeRateUpdater.Infrastructure.Installers;
using ExchangeRateUpdater.Api.Middleware;

namespace ExchangeRateUpdater.Api.Extensions;

public static class ExchangeRateApiInstaller
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApiServices();
        services.AddExchangeRateInfrastructure(configuration, useApiCache: true);
        services.AddOpenTelemetry(configuration, "ExchangeRateUpdaterApi");

        return services;
    }

    public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), includeControllerXmlComments: true);
        });

        return services;
    }

    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
