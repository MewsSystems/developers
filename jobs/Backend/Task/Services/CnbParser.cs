using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExchangeRateUpdater.Services;

public sealed class CnbParser : ICnbParser
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    // detect comma/dot from the CNB file
    public static CultureInfo DetectCulture(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
            return CultureInfo.InvariantCulture;

        var lines = payload.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 3)
            return CultureInfo.InvariantCulture;

        for (int i = 2; i < lines.Length; i++)
        {
            var parts = lines[i].Split('|', StringSplitOptions.TrimEntries);
            if (parts.Length < 5) continue;

            var rateToken = parts[4].Replace("\u00A0", "").Replace(" ", "");
            if (rateToken.IndexOf(',') >= 0) return CultureInfo.GetCultureInfo("cs-CZ"); // comma
            if (rateToken.IndexOf('.') >= 0) return CultureInfo.GetCultureInfo("en-US"); // dot
        }

        return CultureInfo.InvariantCulture;
    }

    public IEnumerable<(string Code, int Amount, decimal Rate)> Parse(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
            yield break;

        var lines = payload.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 3)
            yield break;

        for (int i = 2; i < lines.Length; i++)
        {
            var parts = lines[i].Split('|', StringSplitOptions.TrimEntries);
            if (parts.Length < 5) continue;

            var code = parts[3].Trim().ToUpperInvariant();
            if (!int.TryParse(parts[2], NumberStyles.Integer, Invariant, out var amount) || amount <= 0)
                continue;

            var ratePart = parts[4].Trim().Replace("\u00A0", "").Replace(" ", "");
            var normalizedRate = ratePart.IndexOf(',') >= 0 ? ratePart.Replace(',', '.') : ratePart;

            if (!decimal.TryParse(normalizedRate, NumberStyles.Number, Invariant, out var rate) || rate <= 0)
                continue;

            yield return (code, amount, rate);
        }
    }
}
