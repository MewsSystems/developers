using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace ExchangeRateUpdaterApi.Configuration;

public class SerilogConfiguration
{
    public static Logger Create(string applicationName)
    {
        var configuration = new LoggerConfiguration()
            .Enrich.WithProperty("Application", applicationName)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {Message,-30:lj} {Properties:j}{NewLine}{Exception}]",
                theme: AnsiConsoleTheme.Literate
            );

        var logger = configuration.CreateLogger();
        Log.Logger = logger;

        return logger;
    }
}