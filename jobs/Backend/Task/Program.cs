using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Interfaces;

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
        var services = new ServiceCollection();
        services.AddHttpClient();
        services.AddSingleton<IExchangeRatesService, ExchangeRatesService>();

        var serviceProvider = services.BuildServiceProvider();
        var exchangeRateService = serviceProvider.GetRequiredService<IExchangeRatesService>();

        try
        {
            var exchangeRateResponse = await exchangeRateService.GetExchangeRatesAsync();

            Console.WriteLine($"Successfully retrieved {exchangeRateResponse.ExchangeRates.Count} exchange rates:");
            foreach (var rate in exchangeRateResponse.ExchangeRates)
            {
                Console.WriteLine($"Country: {rate.Country}, Currency: {rate.Currency}, Rate: {rate.Rate}");
            }
        }
        catch (Exception e) 
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'");
        }

        try
        {
            var provider = new ExchangeRateProvider();
            var rates = provider.GetExchangeRates(currencies);

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
