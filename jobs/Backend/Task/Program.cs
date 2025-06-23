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
            Console.WriteLine($"An error occurred while retrieving exchange rates: '{e.Message}'.");
        }
    }
}
