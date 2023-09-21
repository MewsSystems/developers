using ExchangeRateUpdater.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public static class Program
{
    private static IEnumerable<Currency> currencies = new[]
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
        var currentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        // Build configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(currentPath)
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();

        services.AddHttpClient("CNBApiClient", client =>
        {
            var baseUrl = configuration["CNBApi:BaseUrl"];
            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddLogging();
        services.AddMemoryCache();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddTransient<IExchangeRateProvider, CNBExchangeRateProvider>();

        var serviceProvider = services.BuildServiceProvider();
        var exchangeRateProvider = serviceProvider.GetRequiredService<IExchangeRateProvider>();

        try
        {
            var provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();

            var rates = await provider.GetDailyExchangeRateAsync(currencies);

            Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }
}
