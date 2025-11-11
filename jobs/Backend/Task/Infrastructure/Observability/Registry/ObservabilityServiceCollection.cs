using ExchangeRateUpdater.Infrastructure.Observability;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ExchangeRateUpdater.Infrastructure.CNB.Registry
{
    internal static class ObservabilityServiceCollection
    {
        public static IServiceCollection AddObservabilityInfrastructure(this IServiceCollection services)
            => services
                .AddSingleton<Metrics>()
                .AddOpenTelemetry()
                .ConfigureResource(builder => builder.AddService("ExchangeRateUpdater"))
                .WithTracing(builder =>
                {
                    builder.AddHttpClientInstrumentation();
                    builder.AddConsoleExporter();
                })
                .WithMetrics(builder =>
                {
                    builder.AddHttpClientInstrumentation();
                    builder.AddMeter([Metrics.ExchangeRateUpdateMeterName]);
                    builder.AddConsoleExporter();
                }).Services;
    }
}
