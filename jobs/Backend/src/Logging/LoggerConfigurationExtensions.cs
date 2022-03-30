using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Logging;

public static class LoggerConfigurationExtensions
{
    public static void SetupLoggerConfiguration() => Log.Logger = new LoggerConfiguration()
        .ConfigureBaseLogging()
        .CreateLogger();

    public static LoggerConfiguration ConfigureBaseLogging(
        this LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Async(a => a.Console(theme: AnsiConsoleTheme.Code,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message:lj}{NewLine}{Exception:j}"))
            .Enrich.FromLogContext();

        return loggerConfiguration;
    }
}
