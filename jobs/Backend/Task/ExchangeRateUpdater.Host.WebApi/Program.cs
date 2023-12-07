using Serilog;
using ExchangeRateUpdater.Host.WebApi.Configuration;
using YamlDotNet.Serialization;

namespace ExchangeRateUpdater.Host.WebApi;

/// <summary>
/// The starting class of this solution
/// </summary>
public class Program
{
    /// <summary>
    /// Starting method of this solution.
    /// </summary>
    /// <returns></returns>
    public static async Task Main()
    {
        string yamlContent = File.ReadAllText("settings.yaml");
        var deserializer = new DeserializerBuilder().Build();
        ISettings settings = deserializer.Deserialize<Configuration.Settings>(yamlContent);

        var logger = SerilogConfiguration.SetupLogger(settings);
        Log.Logger = logger;
        using var host = new ApplicationHostBuilder(settings, logger).Configure().Build();
        await host.RunAsync();
        logger.Information("Serilog logger setup.");
        await host.WaitForShutdownAsync();
    }
}