using Serilog;
using Serilog.Core.Enrichers;

namespace ExchangeRateUpdater.Host.WebApi.Configuration
{
    internal static class SerilogConfiguration
    {
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
}
