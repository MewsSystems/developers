﻿using ExchangeRateUpdater.Clients.Cnb.Extensions;
using ExchangeRateUpdater.Clients.Cnb.Parsers;
using ExchangeRateUpdater.Console.Configuration;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Console;

public static class Program
{
    private static readonly IEnumerable<Currency> Currencies = new[]
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
        var serviceProvider = ConfigureServices();

        try
        {
            var provider = serviceProvider.GetRequiredService<ExchangeRateProvider>();
            var rates = await provider.GetExchangeRates(Currencies);

            System.Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates)
            {
                System.Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        System.Console.ReadLine();
    }

    private static IServiceProvider ConfigureServices()
    {
        var configuration = AppConfiguration.GetConfiguration();
        IServiceCollection services = new ServiceCollection();

        services.AddTransient<ExchangeRateProvider>();
        services.AddSingleton(configuration);
        services.AddCnbClient(configuration.GetSection("Clients:Cnb").Bind);
        return services.BuildServiceProvider();
    }
}