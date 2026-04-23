using Serilog;
using Serilog.Core.Enrichers;

namespace ExchangeRateUpdater.Host.WebApi.Configuration;

/// <summary>
/// Static Class the configuration Serilog.
/// </summary>
internal static class SerilogConfiguration
{
    /// <summary>
    /// Sets the serilog logger instance.
    /// </summary>
    /// <param name="settings">Insteance of ISettings</param>
    /// <returns></returns>
    internal static Serilog.ILogger SetupLogger(ISettings settings)
    {
         return new LoggerConfiguration()
                        .MinimumLevel.Is(settings.MinimumLogLevel)
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [CorrelationId:{CorrelationId}] {RequestPath}{QueryString} {Message:lj}{NewLine}{Exception}")
                        .Enrich.With(new PropertyEnricher("abc", "a"))
                        .Enrich.FromLogContext()
                        .CreateLogger();
    }
}
