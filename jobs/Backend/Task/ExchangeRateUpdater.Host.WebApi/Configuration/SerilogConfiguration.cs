using Serilog;

namespace ExchangeRateUpdater.Host.WebApi.Configuration
{
    internal static class SerilogConfiguration
    {
        internal static Serilog.ILogger SetupLogger(ISettings settings)
        {
             return new LoggerConfiguration()
                            .MinimumLevel.Is(settings.MinimumLogLevel)
                            .WriteTo.Console()
                            .CreateLogger();
        }
    }
}
