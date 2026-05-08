using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Parses CNB daily exchange rate content.
    /// Format: Plain text, pipe-delimited (Country|Currency|Amount|Code|Rate).
    /// First line = date, second line = header, remaining lines = rates vs CZK.
    /// </summary>
    public static class CnbRatesParser
    {
        private const int HeaderLinesToSkip = 2;
        private const int ExpectedColumnCount = 5;

        // Column indices for: Country|Currency|Amount|Code|Rate
        private const int AmountColumn = 2;
        private const int CodeColumn = 3;
        private const int RateColumn = 4;

        /// <summary>
        /// Parses CNB rates content and returns exchange rates for requested currencies.
        /// </summary>
        /// <param name="content">Raw CNB daily.txt content.</param>
        /// <param name="requestedCodes">Set of currency codes to include (case-insensitive).</param>
        /// <param name="targetCurrency">The target currency (CZK).</param>
        /// <returns>Exchange rates matching requested currencies.</returns>
        public static IEnumerable<ExchangeRate> Parse(string content, HashSet<string> requestedCodes, Currency targetCurrency)
        {
            if (string.IsNullOrWhiteSpace(content))
                yield break;

            var lines = content.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines.Skip(HeaderLinesToSkip))
            {
                var rate = TryParseRateLine(line, requestedCodes, targetCurrency);
                if (rate is not null)
                    yield return rate;
            }
        }

        private static ExchangeRate? TryParseRateLine(string line, HashSet<string> requestedCodes, Currency targetCurrency)
        {
            var columns = line.Split('|');
            if (columns.Length != ExpectedColumnCount)
                return null;

            var code = columns[CodeColumn].Trim();
            if (!requestedCodes.Contains(code))
                return null;

            if (!TryParseDecimal(columns[AmountColumn], out var amount) || amount <= 0)
                return null;

            if (!TryParseDecimal(columns[RateColumn], out var rate))
                return null;

            return new ExchangeRate(new Currency(code), targetCurrency, rate / amount);
        }

        private static bool TryParseDecimal(string value, out decimal result)
        {
            var normalized = value.Trim().Replace(',', '.');
            return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
        }
    }
}

