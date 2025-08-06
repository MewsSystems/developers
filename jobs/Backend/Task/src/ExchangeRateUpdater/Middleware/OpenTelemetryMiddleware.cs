using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using System.Linq;

namespace ExchangeRateUpdater.Middleware;

public static class OpenTelemetryMiddleware
{
    public static IServiceCollection AddOpenTelemetryServices(this IServiceCollection services, IConfiguration configuration)
    {
        var openTelemetryConfig = configuration.GetSection("OpenTelemetry");
        var isOpenTelemetryEnabled = openTelemetryConfig.GetValue<bool>("Enabled", true);
        
        if (!isOpenTelemetryEnabled)
        {
            return services;
        }

        var serviceName = openTelemetryConfig.GetValue<string>("ServiceName", "ExchangeRateUpdater");
        var serviceVersion = openTelemetryConfig.GetValue<string>("ServiceVersion", "1.0.0");
        var resourceAttributes = openTelemetryConfig.GetSection("ResourceAttributes").GetChildren()
            .ToDictionary(x => x.Key, x => (object)(x.Value ?? string.Empty));

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
            .AddAttributes(resourceAttributes);

        // Configure tracing
        var tracingConfig = openTelemetryConfig.GetSection("Tracing");
        var isTracingEnabled = tracingConfig.GetValue<bool>("Enabled", true);

        if (!isTracingEnabled) return services;
        var tracerProviderBuilder = services.AddOpenTelemetry()
            .WithTracing(tracing => tracing.SetResourceBuilder(resourceBuilder));
            
        // Configure HTTP client instrumentation
        var httpClientInstrumentationEnabled = tracingConfig.GetSection("HttpClientInstrumentation").GetValue<bool>("Enabled", true);
        if (httpClientInstrumentationEnabled)
        {
            tracerProviderBuilder.WithTracing(tracing => tracing.AddHttpClientInstrumentation());
        }
            
        // Configure console exporter
        var consoleExporterEnabled = tracingConfig.GetSection("ConsoleExporter").GetValue<bool>("Enabled", true);
        if (consoleExporterEnabled)
        {
            tracerProviderBuilder.WithTracing(tracing => tracing.AddConsoleExporter());
        }

        return services;
    }

    public static ILoggingBuilder AddOpenTelemetryLogging(this ILoggingBuilder logging, IConfiguration configuration)
    {
        var openTelemetryConfig = configuration.GetSection("OpenTelemetry");
        var isOpenTelemetryEnabled = openTelemetryConfig.GetValue<bool>("Enabled", true);
        
        if (!isOpenTelemetryEnabled)
        {
            return logging;
        }

        var loggingConfig = openTelemetryConfig.GetSection("Logging");
        var isLoggingEnabled = loggingConfig.GetValue<bool>("Enabled", true);
        
        if (!isLoggingEnabled)
        {
            return logging;
        }

        var serviceName = openTelemetryConfig.GetValue<string>("ServiceName", "ExchangeRateUpdater");
        var serviceVersion = openTelemetryConfig.GetValue<string>("ServiceVersion", "1.0.0");
        var resourceAttributes = openTelemetryConfig.GetSection("ResourceAttributes").GetChildren()
            .ToDictionary(x => x.Key, x => (object)(x.Value ?? string.Empty));

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
            .AddAttributes(resourceAttributes);

        var consoleExporterEnabled = loggingConfig.GetSection("ConsoleExporter").GetValue<bool>("Enabled", true);
        
        logging.AddOpenTelemetry(options =>
        {
            if (consoleExporterEnabled)
            {
                options.AddConsoleExporter();
            }
            
            options.SetResourceBuilder(resourceBuilder);
        });

        return logging;
    }
} 