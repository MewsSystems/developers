using System;
using System.Globalization;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Domain;

public static class CnbDailyTxtParser
{
    private const string Header = "Country|Currency|Amount|Code|Rate";
    private static readonly CultureInfo CsCulture = CultureInfo.GetCultureInfo("cs-CZ");

    public static IEnumerable<ExchangeRate> Parse(string rawContent)
    {
        ArgumentNullException.ThrowIfNull(rawContent);

        string[] lines = rawContent
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        int headerIndex = Array.FindIndex(lines, line => line == Header);
        if (headerIndex < 0)
        {
            throw new FormatException("Missing CNB header line.");
        }

        for (int i = headerIndex + 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] parts = line.Split('|');
            if (parts.Length != 5)
            {
                throw new FormatException($"Malformed CNB line: '{line}'.");
            }

            if (!decimal.TryParse(parts[2], NumberStyles.Number, CsCulture, out decimal amount))
            {
                throw new FormatException($"Invalid amount in CNB line: '{line}'.");
            }

            if (!decimal.TryParse(parts[4], NumberStyles.Number, CsCulture, out decimal rate))
            {
                throw new FormatException($"Invalid rate in CNB line: '{line}'.");
            }

            string code = parts[3].Trim().ToUpperInvariant();
            yield return new ExchangeRate(new Currency(code), new Currency("CZK"), rate / amount);
        }
    }
}
