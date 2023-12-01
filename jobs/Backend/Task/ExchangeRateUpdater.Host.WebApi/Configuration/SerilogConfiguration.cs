using Serilog;

namespace ExchangeRateUpdater.Host.WebApi.Configuration
{
    internal static class SerilogConfiguration
    {
        internal static Serilog.ILogger SetupLogger()
        {
             return new LoggerConfiguration()
                            .WriteTo.Console()
                            .CreateLogger();
        }
    }
}
