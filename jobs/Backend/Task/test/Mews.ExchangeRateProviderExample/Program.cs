using Mews.ExchangeRateProvider;
using Mews.ExchangeRateProvider.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;
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
            var configuration = InitialiseConfiguration();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.File(new JsonFormatter(), $"Mews.{nameof(ExchangeRateProviderExample)}.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var serviceProvider = BuildServiceProvider(configuration);

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

    private static IServiceProvider BuildServiceProvider(IConfigurationRoot configuration) =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
                services.AddExchangeRateProvider(configuration.GetRequiredSection(CzechNationalBankExchangeRateProviderOptions.Section)))
            .UseSerilog()
            .Build()
            .Services;

    private static IConfigurationRoot InitialiseConfiguration() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ENVIRONMENT_NAME") ?? "local"}.json", optional: true)
            .Build();

    private static void WriteExchangeRatesToConsole(IEnumerable<ExchangeRate> rates)
    {
        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
        foreach (var rate in rates.OrderBy(r => r.SourceCurrency.Code))
        {
            Console.WriteLine(rate.ToString());
        }
    }
}