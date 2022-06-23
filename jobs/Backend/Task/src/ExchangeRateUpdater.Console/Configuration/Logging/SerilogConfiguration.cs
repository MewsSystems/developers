using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ExchangeRateUpdater.Console.Configuration.Logging;

internal class SerilogConfiguration
{
    internal ILogger Create(LogEventLevel minimumLogLevel, string httpLogEndpoint)
    {
        var configuration = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .MinimumLevel.Is(minimumLogLevel)
                            .WriteTo.Console(
                                             outputTemplate:
                                             "[{Timestamp:HH:mm:ss} {Level:u3}] {Message,-30:lj} {Properties:j}{NewLine}{Exception}",
                                             theme: AnsiConsoleTheme.Literate)
                            .WriteTo.Seq(httpLogEndpoint);

        return configuration.CreateLogger();
    }
}