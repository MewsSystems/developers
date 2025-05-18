using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

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
        }
    }
}