using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        try
        {
            var serviceProvider = CreateServiceCollection();
            
            var exchangeRateProvider = serviceProvider.GetService<IExchangeRateProvider>();
            var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

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

    public static ServiceProvider CreateServiceCollection()
    {
        var services = new ServiceCollection();
        services.AddTransient<HttpClient>();
        services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;

    }
}

