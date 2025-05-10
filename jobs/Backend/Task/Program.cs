using ExchangeRateUpdater.Configuration;
using Microsoft.Extensions.Configuration;
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
        new Currency("XYZ")
    };

    public async static Task Main(string[] args)
    {
        try
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var provider = new ExchangeRateProvider();
            var rates = await ExchangeRateProvider.GetExchangeRatesAsync(currencies);

            // Bind CnbApiSettings from configuration
            var cnbApiSettings = config.GetSection("CnbApiSettings").Get<CnbApiSettings>();
            if (string.IsNullOrWhiteSpace(cnbApiSettings.BaseAddress) ||
                string.IsNullOrWhiteSpace(cnbApiSettings.Endpoint) ||
                string.IsNullOrWhiteSpace(cnbApiSettings.BaseCurrency))
            {
                throw new InvalidOperationException("One or more required configuration settings are missing.");
            }

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(cnbApiSettings.BaseAddress),
                Timeout = TimeSpan.FromSeconds(30)
            };
            var cnbProvider = new CnbApiExchangeRateDataProvider(httpClient, cnbApiSettings);

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
