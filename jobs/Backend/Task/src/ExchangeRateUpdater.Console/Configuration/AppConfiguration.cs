using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Console.Configuration;

/// <summary>
/// Application configuration.
/// </summary>
internal static class AppConfiguration
{
    /// <summary>
    /// Adds appsettings file.
    /// </summary>
    internal static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
        return builder.AddJsonFile("appsettings.json").Build();
    }
}