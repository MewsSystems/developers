using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;  
using System.Threading.Tasks;
using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// TOP DECISIONS:
    /// - Settings for Environment-specific configuration
    /// - Dependency Injection for testability and loose coupling
    /// - IHttpClientFactory pattern to prevent socket exhaustion
    /// </summary>
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

        public static async Task Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
            var services = ConfigureServices(environment);

            try
            {
                var provider = services.GetRequiredService<ExchangeRateProvider>();
                var rates = await provider.GetExchangeRatesAsync(currencies);

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

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadLine();
        }

        private static ServiceProvider ConfigureServices(string environment)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            // Options Pattern: Strongly-typed configuration with validation support, Type-safe, compile-time checking
            services.Configure<ExchangeRateApiSettings>(configuration.GetSection("ExchangeRateApi"));

            // HttpClient and Parser with generic interfaces for easy abstraction and testing, and swappable with other implementations of api or parser
            services.AddHttpClient<IExchangeRateApiClient, CnbApiClient>();
            services.AddTransient<IExchangeRateDataParser, CnbDataParser>();

            // Provider has no interface: It's already generic, just orchestrates dependencies. Contains zero business logic
            services.AddTransient<ExchangeRateProvider>();

            return services.BuildServiceProvider();
        }
    }
}