using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ExchangeRateUpdater.Configuration;

public static class OpenTelemetryConfiguration
{
    /// <summary>
    /// This extension method is used for configuring OpenTelemetry tracing, metrics, and logging.
    /// It ensures distributed tracing and observability across the application.
    /// </summary>

    public static void ConfigureOpenTelemetry(IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(serviceName: "ExchangeRateUpdater"))
            .WithTracing(tracing =>
            {
                tracing
                   .AddSource("ExchangeRateService")
                   .AddHttpClientInstrumentation()
                   .AddAspNetCoreInstrumentation()
                   .AddConsoleExporter();
            })
            .WithMetrics(metrics =>
             {
                metrics
                .AddMeter("ExchangeRateService")
                .AddConsoleExporter();
            })
            .WithLogging(logging =>
            {
                logging.AddConsoleExporter();
            });
    }

}
