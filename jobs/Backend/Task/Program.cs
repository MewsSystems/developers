using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ExchangeRateUpdater.Configuration;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using ExchangeRateUpdater.Common.Constants;
using ExchangeRateUpdater.ExchangeRate.Providers;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater
{
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
            // Setup DI
            var services = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            var exchangeRateProvider = provider.GetRequiredService<IExchangeRateService>();
            var memoryCache = provider.GetRequiredService<IMemoryCache>();

            // Show initial rates for default currencies
            try
            {
                Console.WriteLine("Fetching exchange rates for default currencies...");
                var initialRates = await exchangeRateProvider.GetExchangeRateAsync(currencies.ToList());
                if (initialRates == null || initialRates.Count == 0)
                {
                    Console.WriteLine("Sorry, we couldn't retrieve any exchange rates at this time. Please try again later.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nSuccessfully retrieved {initialRates.Count} exchange rates:");

                    foreach (var rate in initialRates)
                    {
                        Console.WriteLine(rate.ToString());
                    }
                }
            }
            catch (Exception)
            {
               
               Console.ForegroundColor = ConsoleColor.Red;
               Console.WriteLine("\nSorry, something went wrong while fetching exchange rates. Please try again later.");
               Console.ResetColor();
            }

            // User interaction loop
            while (true)
            {
                Console.ResetColor();
                Console.Write("\nEnter currency codes (comma separated), or type 'clear' to clear cache: ");
                var input = Console.ReadLine();

                // Do not exit on empty input, just prompt again
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.Trim().Equals("clear", StringComparison.OrdinalIgnoreCase))
                {
                    memoryCache.Remove(AppConstants.DailyRatesCacheKey);
                    memoryCache.Remove(AppConstants.MonthlyRatesCacheKey);
                    Console.WriteLine("Cache cleared.");
                    continue;
                }

                var codes = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                 .Select(code => code.ToUpperInvariant())
                                 .ToList();

                // ISO 4217 format: exactly 3 uppercase letters
                var iso4217Regex = new Regex("^[A-Z]{3}$");
                var invalidCodes = codes.Where(code => !iso4217Regex.IsMatch(code)).ToList();

                if (invalidCodes.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid ISO 4217 currency codes: {string.Join(", ", invalidCodes)}");
                    continue;
                }

                var currencyObjects = codes.Select(code => new Currency(code)).ToList();

                try
                {
                    var rates = await exchangeRateProvider.GetExchangeRateAsync(currencyObjects);
                    if (rates == null || rates.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Sorry, we couldn't find exchange rates for your request.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\nSuccessfully retrieved {rates.Count} exchange rates:");
                        foreach (var rate in rates)
                        {
                            Console.WriteLine(rate.ToString());
                        }
                        Console.ResetColor();
                    }
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nSorry, something went wrong while fetching exchange rates. Please try again later.");
                    Console.ResetColor();
                }
            }
        }

    }
}
