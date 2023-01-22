using ExchangeRateUpdater.BankRatesManagers;
using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public static class Program
{
    private static readonly IEnumerable<Currency> currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ"),
        new Currency("AUD")
    };

    public static async Task Main(string[] args)
    {
        //// Service provider of application
        var httpClient = new HttpClient();
        var cnbRatesManager = new CnbRatesManager();
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, cnbRatesManager);
        //// End of service provider

        try
        {
            var rates = await exchangeRateProvider.GetExchangeRates(currencies);

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