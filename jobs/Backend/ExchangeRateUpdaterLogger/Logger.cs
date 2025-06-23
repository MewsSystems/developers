using Serilog;

namespace ExchangeRateLogger
{
    public class Logger
    {
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        }
        public static void CloseAndFlush()
        {
            Log.CloseAndFlush();
        }
    }
}
