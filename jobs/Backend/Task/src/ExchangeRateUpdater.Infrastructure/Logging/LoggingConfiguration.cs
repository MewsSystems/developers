using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;
using Serilog.Sinks.Grafana.Loki;

namespace ExchangeRateUpdater.Infrastructure.Logging;

public static class LoggingConfiguration
{
    public static IHostBuilder ConfigureSerilogLogging(this IHostBuilder builder)
    {
        return builder.UseSerilog((context,
            loggerConfig) =>
        {
            var assembly = Assembly.GetEntryAssembly()?.
                GetName().
                Name;

            ConfigureSerilog(loggerConfig, context.Configuration, context.HostingEnvironment.EnvironmentName, assembly);
        });
    }

    public static void ConfigureSerilog(
        LoggerConfiguration loggerConfig,
        IConfiguration configuration,
        string environmentName,
        string? applicationName = null)
    {
        var logFilePath = configuration["Serilog:LogFilePath"];
        var lokiUrl = configuration["Serilog:LokiUrl"] ?? "http://localhost:3100";

        // Base configuration
        loggerConfig.ReadFrom.Configuration(configuration).
            Enrich.FromLogContext().
            Enrich.WithMachineName().
            Enrich.WithProperty("Environment", environmentName).
            Enrich.WithProperty("Application", applicationName ?? "ExchangeRateUpdater");

        // Configure for development (human-readable)
        if (string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase))
        {
            loggerConfig.WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}").
                MinimumLevel.Debug();

            // Add file logging for development (text format)
            if (!string.IsNullOrEmpty(logFilePath))
                loggerConfig.WriteTo.File(
                    logFilePath,
                    rollingInterval: RollingInterval.Day,
                    shared: true);

            // Add Loki sink for development
            loggerConfig.WriteTo.GrafanaLoki(
                lokiUrl,
                labels: new[]
                {
                    new LokiLabel { Key = "app", Value = applicationName ?? "ExchangeRateUpdater" },
                    new LokiLabel { Key = "environment", Value = environmentName }
                },
                propertiesAsLabels: new[] { "Level", "Environment", "Application" },
                batchPostingLimit: 100,
                period: TimeSpan.FromSeconds(2),
                textFormatter: new MessageTemplateTextFormatter("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"));
        }
        // Configure for production (JSON-formatted)
        else
        {
            loggerConfig.WriteTo.Console(new CompactJsonFormatter()).
                MinimumLevel.Information().
                MinimumLevel.Override("Microsoft", LogEventLevel.Warning).
                MinimumLevel.Override("System", LogEventLevel.Warning);

            // Add file logging for production (JSON format)
            if (!string.IsNullOrEmpty(logFilePath))
                loggerConfig.WriteTo.File(
                    new JsonFormatter(renderMessage: true),
                    logFilePath,
                    rollingInterval: RollingInterval.Day,
                    shared: true);

            // Add Loki sink for production
            loggerConfig.WriteTo.GrafanaLoki(
                lokiUrl,
                labels: new[]
                {
                    new LokiLabel { Key = "app", Value = applicationName ?? "ExchangeRateUpdater" },
                    new LokiLabel { Key = "environment", Value = environmentName }
                },
                propertiesAsLabels: new[] { "Level", "Environment", "Application" },
                batchPostingLimit: 100,
                period: TimeSpan.FromSeconds(2),
                textFormatter: new JsonFormatter(renderMessage: true));
        }
    }
}