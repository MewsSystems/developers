using ExchangeRateUpdater.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExchangeRateUpdater;

public static class ExtensionMethods
{
    /// <summary>
    /// Returns a tuple of valid currencies and a warning message if any invalid currencies were found
    /// </summary>
    /// <param name="currencies"></param>
    /// <returns></returns>
    public static (IEnumerable<ValidCurrency> validCurrencies, string? warningMessage) Validate(this IEnumerable<Currency> currencies)
    {
        var validCurrencies = new List<ValidCurrency>();
        var warningMessage = new StringBuilder("Invalid currencies found:\n");
        if (currencies != null)
            foreach (var currency in currencies)
            {
                if (ValidCurrency.TryCreateFromCurrency(currency, out var validCurrency))
                {
                    validCurrencies.Add(validCurrency);
                    continue;
                }
                warningMessage.AppendLine($"- {currency.Code}");
            }
        return (validCurrencies, warningMessage.Length > 0 ? warningMessage.ToString() : null);
    }

    /// <summary>
    /// Prints the response to console along with any eventual warning or error messages
    /// </summary>
    /// <param name="response"></param>
    public static void PrintToConsole(this NonNullResponse<IEnumerable<ExchangeRate>> response)
    {
        if (response.IsSuccess)
        {
            response.Content.IfAnyPrintWithTitle($"Successfully retrieved {response.Content.Count()} exchange rates:\n");
        }
        else
        {
            Console.WriteLine("Could not retrieve exchange rates.");
        }

        response.Errors.IfAnyPrintWithTitle("\nErrors:");
        response.Warnings.IfAnyPrintWithTitle("\nWarnings:");
    }

    private static void IfAnyPrintWithTitle<T>(this IEnumerable<T> messages,string title)
    {
        if (!messages.Any()) 
            return;

        Console.WriteLine($"{title}");
        foreach (var message in messages)
        {
            Console.WriteLine(message);
        }
    }
}