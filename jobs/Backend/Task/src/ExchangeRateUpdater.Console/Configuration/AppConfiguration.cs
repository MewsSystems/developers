using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Console.Configuration;

internal static class AppConfiguration
{
    internal static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
        return builder.AddJsonFile("appsettings.json").Build();
    }
}