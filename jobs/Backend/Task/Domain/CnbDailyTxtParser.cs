using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Domain;

public static class CnbDailyTxtParser
{
    private const string Header = "Country|Currency|Amount|Code|Rate";
    private const int MaxLinePreviewLength = 100;
    private static readonly CultureInfo CsCulture = CultureInfo.GetCultureInfo("cs-CZ");

    public static IEnumerable<ExchangeRate> Parse(string rawContent)
    {
        ArgumentNullException.ThrowIfNull(rawContent);

        string[] lines = rawContent
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n');

        int headerIndex = Array.FindIndex(lines, line => line.Trim() == Header);
        if (headerIndex < 0)
        {
            throw new FormatException("Missing CNB header line.");
        }

        for (int i = headerIndex + 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] parts = line.Split('|');
            if (parts.Length != 5)
            {
                throw CreateFormatException(i + 1, line, "Malformed CNB line.");
            }

            if (!decimal.TryParse(parts[2].Trim(), NumberStyles.Number, CsCulture, out decimal amount))
            {
                throw CreateFormatException(i + 1, line, "Invalid amount in CNB line.");
            }

            if (!decimal.TryParse(parts[4].Trim(), NumberStyles.Number, CsCulture, out decimal rate))
            {
                throw CreateFormatException(i + 1, line, "Invalid rate in CNB line.");
            }

            if (amount <= 0)
            {
                throw CreateFormatException(i + 1, line, "Amount must be greater than zero.");
            }

            if (rate <= 0)
            {
                throw CreateFormatException(i + 1, line, "Rate must be greater than zero.");
            }

            string code = parts[3].Trim().ToUpperInvariant();
            yield return new ExchangeRate(new Currency(code), new Currency("CZK"), rate / amount);
        }
    }

    private static FormatException CreateFormatException(int lineNumber, string lineContent, string reason)
    {
        string preview = lineContent.Length <= MaxLinePreviewLength
            ? lineContent
            : string.Concat(lineContent.Take(MaxLinePreviewLength), "...");

        return new FormatException($"{reason} Line {lineNumber}: '{preview}'");
    }
}
