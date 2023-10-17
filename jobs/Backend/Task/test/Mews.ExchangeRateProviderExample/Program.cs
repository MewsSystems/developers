using Mews.ExchangeRateProvider;
using Mews.ExchangeRateProvider.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mews.ExchangeRateProviderExample;

public static class Program
{
    private static IEnumerable<Currency> _currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };

    public static async Task Main(string[] args)
    {
        try
        {
            var serviceProvider = BuildServiceProvider(InitialiseConfiguration());

            using var scope = serviceProvider.CreateScope();
            var rateProvider = scope.ServiceProvider.GetRequiredService<IExchangeRateProvider>();
            var rates = await rateProvider.GetExchangeRatesAsync(_currencies, CancellationToken.None);

            WriteExchangeRatesToConsole(rates);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }

    private static IConfigurationRoot InitialiseConfiguration() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ENVIRONMENT_NAME") ?? "local"}.json", optional: true)
            .Build();

    private static ServiceProvider BuildServiceProvider(IConfigurationRoot configuration) =>
        new ServiceCollection()
            .AddExchangeRateProvider(configuration.GetRequiredSection(ExchangeRateProviderOptions.Section))
            .BuildServiceProvider();

    private static void WriteExchangeRatesToConsole(IEnumerable<ExchangeRate> rates)
    {
        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
        foreach (var rate in rates.OrderBy(r => r.SourceCurrency.Code))
        {
            Console.WriteLine(rate.ToString());
        }
    }
}