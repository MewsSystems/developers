using System;
using System.Collections.Generic;
using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

public class Program
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

    public static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>()
            .AddLogging(builder => builder.AddConsole())
            .AddHttpClient<ICzechNationalBankExchangeRateClient, CzechNationalBankExchangeRateClient>(x =>
                x.BaseAddress = new Uri("https://api.cnb.cz/"))
            .Services
            .BuildServiceProvider();
        var logger = serviceProvider.GetService<ILoggerFactory>()
            .CreateLogger<Program>();

        try
        {
            var provider = serviceProvider.GetService<IExchangeRateProvider>();
            var rates = provider.GetExchangeRates(_currencies);

            Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not retrieve exchange rates: '{message}'.", e.Message);
        }

        Console.ReadLine();
    }
}