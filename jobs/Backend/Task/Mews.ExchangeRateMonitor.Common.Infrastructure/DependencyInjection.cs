using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Mews.ExchangeRateMonitor.Common.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddCoreInfrastructure(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddHostOpenTelemetry();

        return services;
    }

    private static IServiceCollection AddHostOpenTelemetry(
        this IServiceCollection services,
        params string[] activityModuleNames)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("Mews.ExchangeRateMonitor"))
            .WithTracing(tracing =>
            {
                tracing
                    //creates spans(operation activities) for incoming http requests
                    .AddAspNetCoreInstrumentation()
                    //creates spans(operation activities) for outgoing http requests
                    .AddHttpClientInstrumentation()
                    //Sends trace data to an OTLP endpoint (usually an OpenTelemetry Collector).
                    //The collector then forwards it to Jaeger
                    //OTLP exporter config(endpoint, protocol, headers) comes from appsettings.json or env vars
                    .AddOtlpExporter();
            });

        return services;
    }
}
