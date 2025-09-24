using ExchangeRateUpdater.Console.Services;
using ExchangeRateUpdater.Core;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.Globalization;

namespace ExchangeRateUpdater.Console;

public static class Program
{
    private static readonly IEnumerable<Currency> DefaultCurrenciesList = new[]
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
    };

    public static async Task Main(string[] args)
    {
        // Define command line options
        var dateOption = new Option<DateTime?>(
            "--date",
            description: "The date to fetch exchange rates for (format: yyyy-MM-dd). Defaults to today.",
            parseArgument: result =>
            {
                if (result.Tokens.Count == 0)
                    return null;
                
                var dateString = result.Tokens.Single().Value;
                if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    return date;
                }
                
                result.ErrorMessage = $"Invalid date format. Please use yyyy-MM-dd format (e.g., 2023-12-01).";
                return null;
            });

        var currenciesOption = new Option<string[]>(
            "--currencies",
            description: "Comma-separated list of currency codes to fetch (e.g., USD,EUR,JPY). Defaults to predefined list.",
            parseArgument: result =>
            {
                if (result.Tokens.Count == 0)
                    return Array.Empty<string>();
                
                var currenciesString = result.Tokens.Single().Value;
                return currenciesString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(c => c.Trim().ToUpperInvariant())
                                     .ToArray();
            });

        // Create root command
        var rootCommand = new RootCommand("Fetches exchange rates from the Czech National Bank.")
        {
            dateOption,
            currenciesOption
        };

        rootCommand.SetHandler(async (DateTime? date, string[] currencyCodes) =>
        {
            await RunExchangeRateUpdaterAsync(date, currencyCodes);
        }, dateOption, currenciesOption);

        // Parse and execute
        await rootCommand.InvokeAsync(args);
    }

    private static async Task RunExchangeRateUpdaterAsync(DateTime? date, string[] currencyCodes)
    {
        try
        {
            var currenciesToFetch = GetCurrenciesToFetch(currencyCodes);
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Add Core services
            var services = new ServiceCollection();
            services.AddExchangeRateCoreDependencies(configuration);
            services.AddScoped<IExchangeRateCache, NoOpExchangeRateCache>();

            // Build service provider
            var serviceProvider = services.BuildServiceProvider();
            var exchangeRateService = serviceProvider.GetRequiredService<IExchangeRateService>();

            System.Console.WriteLine("=== Exchange Rate Updater ===");
            System.Console.WriteLine();

            var rates = await exchangeRateService.GetExchangeRates(currenciesToFetch, date.AsMaybe());

            var rateList = rates.ToList();
            if (rateList.Any())
            {
                System.Console.WriteLine($"Successfully retrieved {rateList.Count} exchange rates:");
                System.Console.WriteLine();

                foreach (var rate in rateList)
                {
                    System.Console.WriteLine($"  {rate}");
                }
            }
            else
            {
                System.Console.WriteLine("No exchange rates were found for the requested currencies.");
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                System.Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            System.Console.WriteLine();
            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
    }

    private static IEnumerable<Currency> GetCurrenciesToFetch(string[] currencyCodes)
    {
        if (currencyCodes != null && currencyCodes.Length > 0)
        {
            var currencies = new List<Currency>();
            foreach (var code in currencyCodes)
            {
                try
                {
                    currencies.Add(new Currency(code));
                }
                catch (ArgumentException ex)
                {
                    System.Console.WriteLine($"Warning: Invalid currency code '{code}' - {ex.Message}");
                }
            }

            if (currencies.Any())
            {
                return currencies;
            }
        }

        System.Console.WriteLine("Warning: No valid currencies provided, using default set.");
        return DefaultCurrenciesList;
    }
}
