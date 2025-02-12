using ExchangeRateUpdater.Application.Exceptions;
using ExchangeRateUpdater.Application.Queries;
using ExchangeRateUpdater.Domain.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationException = ExchangeRateUpdater.Application.Exceptions.ApplicationException;

namespace ExchangeRateUpdater.ConsoleApp.Services;

/// <summary>
/// Handles the console-based interaction for fetching exchange rates.
/// </summary>
public class ExchangeRateConsoleService
{
    private readonly ISender _sender;
    private readonly ILogger<ExchangeRateConsoleService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeRateConsoleService"/> class.
    /// </summary>
    /// <param name="sender">MediatR sender instance.</param>
    /// <param name="logger">Logger instance.</param>
    public ExchangeRateConsoleService(ISender sender, ILogger<ExchangeRateConsoleService> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    /// Starts the console application, prompting the user for input and retrieving exchange rates.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Welcome to the Exchange Rate Updater Console App!");
        Console.WriteLine("-------------------------------------------------");

        while (true)
        {
            Console.Write("\nEnter a date (yyyy-MM-dd) or press Enter for today: ");
            var inputDate = Console.ReadLine()?.Trim();

            if (!TryGetValidDate(inputDate, out DateTime date))
            {
                ConsoleWriteWithColor("Invalid date format. Please use yyyy-MM-dd.", ConsoleColor.Red);
                continue;
            }

            Console.Write("Enter currency codes separated by commas (e.g., USD,EUR) or press Enter for all: ");
            var inputCurrencies = Console.ReadLine()?.Trim();
            var currencies = inputCurrencies?
                                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(c => c.Trim().ToUpperInvariant())
                                            .ToList();

            await FetchAndDisplayExchangeRatesAsync(date, currencies, cancellationToken);

            Console.Write("\nWould you like to fetch another exchange rate? (y/n): ");
            if (!Console.ReadLine()?.Trim().Equals("y", StringComparison.OrdinalIgnoreCase) ?? false)
            {
                ConsoleWriteWithColor("Exiting application. Goodbye!", ConsoleColor.Cyan);
                break;
            }
        }
    }

    /// <summary>
    /// Validates user input for date format.
    /// </summary>
    private static bool TryGetValidDate(string? inputDate, out DateTime date)
    {
        if (string.IsNullOrWhiteSpace(inputDate))
        {
            date = DateTime.Today;
            return true;
        }

        return DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }

    /// <summary>
    /// Writes a message to the console with a specified color and resets it afterward.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="color">The console color to use.</param>
    private static void ConsoleWriteWithColor(string message, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = originalColor;
    }

    /// <summary>
    /// Fetches and displays exchange rates.
    /// </summary>
    private async Task FetchAndDisplayExchangeRatesAsync(DateTime date, IEnumerable<string>? currencies, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetExchangeRatesQuery { Currencies = currencies?.ToList(), Date = date };
            var exchangeRatesResponse = await _sender.Send(query, cancellationToken);

            Console.WriteLine($"\n Exchange Rates for {date:yyyy-MM-dd} (Base Currency: {CurrencyConstants.SourceCurrencyCode}):");
            Console.WriteLine("-----------------------------------------------------");

            foreach (var rate in exchangeRatesResponse.ExchangeRates)
            {
                Console.WriteLine(rate.ToString());
            }

            if (exchangeRatesResponse.MissingCurrencies != null && exchangeRatesResponse.MissingCurrencies.Any())
            {
                ConsoleWriteWithColor("\nSome currencies were not found:", ConsoleColor.Yellow);
                Console.WriteLine(string.Join(", ", exchangeRatesResponse.MissingCurrencies));
            }
        }
        catch (ValidationException ex)
        {
            HandleException(ex.Title, ex.Message, ex.ErrorsDictionary);
        }
        catch (ApplicationException ex)
        {
            HandleException(ex.Title, ex.Message);
        }
        catch (Exception ex)
        {
            HandleException("Unexpected Error", ex.Message);
        }
    }

    /// <summary>
    /// Handles and logs exceptions.
    /// </summary>
    private void HandleException(string title, string message, IReadOnlyDictionary<string, string[]>? errors = null)
    {
        _logger.LogError("{Title}: {Message}", title, message);
        ConsoleWriteWithColor($"\n {title}: {message}", ConsoleColor.Red);

        if (errors != null)
        {
            foreach (var error in errors)
            {
                ConsoleWriteWithColor($" {error.Key}: {string.Join(", ", error.Value)}", ConsoleColor.Red);
            }
        }
    }
}
