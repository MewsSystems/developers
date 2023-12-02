using Serilog;
using ExchangeRateUpdater.Host.WebApi.Configuration;

namespace ExchangeRateUpdater.Host.WebApi;

public class Program
{
    public static async Task Main()
    {
        var logger = SerilogConfiguration.SetupLogger();

        Log.Logger = logger;
        var host = new ApplicationHostBuilder().Configure().Build();
        await host.RunAsync();
        logger.Information("Serilog logger setup.");
        await host.WaitForShutdownAsync();

    }
}