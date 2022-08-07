using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace ExchangeRateUpdater.Host.Console.Configuration.Logging;

internal static class SerilogConfiguration
{
    public static Logger Create(string applicationName, ISettings settings)
    {
        var configuration = new LoggerConfiguration()
            .MinimumLevel.Is(settings.MinimumLogLevel)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", applicationName)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message,-30:lj} {Properties:j}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Literate)
            .WriteTo.Seq(settings.HttpLogEndpoint);
        
        var logger = configuration.CreateLogger();

        // Set the Serilog singleton logger reference so CloseAndFlush works correctly
        Log.Logger = logger;

        return logger;
    }
}