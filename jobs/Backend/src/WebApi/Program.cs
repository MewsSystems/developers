using Logging;
using Serilog;
using LoggerConfigurationExtensions = Logging.LoggerConfigurationExtensions;

namespace ExchangeRate.WebApi;

public static class Program
{
    public static int Main()
    {
        const string appName = "ExchangeRate web API";

        try
        {
            // Console logging for used for troubleshooting before getting all required info for logger
            LoggerConfigurationExtensions.SetupLoggerConfiguration();

            Log.Information("Starting web host {AppName}", appName);
            CreateHostBuilder().Build().Run();
            Log.Information("Ending web host {AppName}", appName);
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly {AppName}", appName);
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .UseSerilog((_, _, loggerConfiguration) =>
            {
                loggerConfiguration.ConfigureBaseLogging();
                // Potentially add cloud logging (AppInsights for Azure, AWS something....)
            })
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}
