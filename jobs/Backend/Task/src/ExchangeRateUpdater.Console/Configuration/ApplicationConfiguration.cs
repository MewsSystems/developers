using System.IO;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Console.Configuration;

internal static class ApplicationConfiguration
{
    internal static IConfiguration GetConfiguration()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());

        return builder.AddJsonFile("appsettings.json")
                      .Build();
    }
}