using Serilog;
using Serilog.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Mews.ExchangeRate.API.ConfigurationExtensionMethods;

[ExcludeFromCodeCoverage(Justification = "This class provides a set of extension methods for configuring serilog and it´s not testable by unit tests")]
internal static class LoggerExtensionMethods
{
    public static ReloadableLogger CreateMewsBootstrapLogger(this LoggerConfiguration loggerConfig,
        string serviceName)
    {
        return loggerConfig
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .Enrich.WithProperty("Service name", serviceName)
            .CreateBootstrapLogger();
    }

    public static IHostBuilder AddMewsLoggingConfiguration(this IHostBuilder builder,
        string serviceName)
    {
        return builder.UseSerilog((hostContext, serviceProvider, loggerConfiguration) =>
        {
            loggerConfiguration
              .MinimumLevel.Information()
              .Enrich.WithProperty("Service name", serviceName)
              .Enrich.FromLogContext()
              .WriteTo.Console();
        });
    }
}
