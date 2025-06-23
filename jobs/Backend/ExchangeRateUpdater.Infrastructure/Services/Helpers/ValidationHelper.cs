using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Infrastructure.Configuration;

namespace ExchangeRateUpdater.Infrastructure.Services.Helpers
{
    internal class ValidationHelper
    {
        /// <summary>
        /// Validates the format of the given date string based on the specified scope.
        /// Validates formats such as "yyyy-MM-dd", "yyyy-MM", and "yyyy" for daily, yearly, or monthly exchange rates.
        /// </summary>
        /// <param name="scope">The scope of the validation (e.g., Daily, DailyYear, DailyCurrencyMonth).</param>
        /// <param name="date">The date string to validate.</param>
        /// <param name="formatException">The exception containing an error message if the date format is invalid.</param>
        /// <returns>True if the date format is valid, otherwise false.</returns>
        internal static bool ValidateDateFormat(string scope, ref string? date, out FormatException? formatException) => scope switch
        {
            ExchangeRateRoutes.Daily => TryParseAndHandleException(ExchangeRateDateFormats.FullDate, ref date, out formatException),
            ExchangeRateRoutes.DailyYear => TryParseAndHandleException(ExchangeRateDateFormats.Year, ref date, out formatException),
            ExchangeRateRoutes.DailyCurrencyMonth => TryParseAndHandleException(ExchangeRateDateFormats.YearMonth, ref date, out formatException),
            _ => TryParseAndHandleException(string.Empty, ref date, out formatException),
        };

        /// <summary>
        /// Tries to parse the given date string in the specified format and returns whether the parsing was successful.
        /// If parsing fails, it sets a format exception with the error message.
        /// </summary>
        /// <param name="format">The date format to validate against.</param>
        /// <param name="date">The date string to validate.</param>
        /// <param name="formatException">The exception that will be set if parsing fails.</param>
        /// <returns>True if the date string matches the expected format, otherwise false.</returns>
        private static bool TryParseAndHandleException(string format, ref string? date, out FormatException? formatException)
        {
            formatException = null;
            date = string.IsNullOrEmpty(date) ? DateTime.Today.ToString(format) : date;
            if (DateOnly.TryParseExact(date, format, out _))
                return true;
            formatException = new FormatException($"Invalid date format. Input:'{date}'. Expected format: {format}");
            return false;

        }


        /// <summary>
        /// Validates the currency code to follow the ISO 4217 standard (three uppercase alphabetic characters).
        /// </summary>
        /// <param name="currencyCode">The currency code to validate.</param>
        /// <param name="formatException">The exception that will be set if the currency code is invalid.</param>
        /// <returns>True if the currency code is valid, empty or null, otherwise false.</returns>
        internal static bool ValidateCurrency(string? currencyCode, out FormatException? formatException)
        {
            formatException = null;

            if (string.IsNullOrEmpty(currencyCode) || currencyCode.Length == 3 && currencyCode.All(char.IsUpper))
                return true;

            formatException = new FormatException($"Invalid currency codes found. Input:'{currencyCode}' Currency codes should follow ISO 4217 standard (e.g., 'USD', 'EUR').");
            return false;
        }

        /// <summary>
        /// Validates a list of currency codes to ensure each one follows the ISO 4217 standard (three uppercase alphabetic characters).
        /// </summary>
        /// <param name="currencies">The collection of currency objects to validate.</param>
        /// <param name="formatException">The exception that will be set if any of the currency codes are invalid.</param>
        /// <returns>True if all currency codes are valid or the list is empty or null, otherwise false.</returns>
        internal static bool ValidateCurrency(IEnumerable<Currency>? currencies, out FormatException? formatException)
        {
            formatException = null;

            if (currencies is null)
                return true;

            foreach (var currency in currencies)
            {
                if (!ValidateCurrency(currency.ToString(), out formatException))
                    return false;
            }

            return true;
        }
    }
}
