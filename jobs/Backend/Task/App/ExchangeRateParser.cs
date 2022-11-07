using System;
using System.Collections.Generic;
using System.Globalization;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.App;

public static class ExchangeRateParser
{
    private const int CurrencyCodeIndex = 3;
    private const int ExchangeRateIndex = 4;
    private const int StartRateLineIndex = 2;
    private static readonly Currency DefaultCurrency = new Currency("CZK");
    private static readonly CultureInfo CzechCultureInfo = new CultureInfo("cs-CZ");

    public static IEnumerable<ExchangeRate> ParseExchangeRates(string data)
    {
        if (string.IsNullOrEmpty(data))
            yield break;

        var lines = data.Split('\n');

        for (var i = StartRateLineIndex; i < lines.Length; i++)
        {
            var rate = ParseLine(lines[i]);
            if (rate is null)
                continue;

            yield return rate;
        }
    }

    private static ExchangeRate? ParseLine(string line)
    {
        if (string.IsNullOrEmpty(line))
            return null;

        var columns = line.Split('|');
        if (columns.Length != 5)
            return null;

        var currency = new Currency(columns[CurrencyCodeIndex]);

        var result = decimal.TryParse(columns[ExchangeRateIndex], NumberStyles.Any, CzechCultureInfo, out decimal rate);
        if (!result)
            throw new Exception($"Rate value: {columns[ExchangeRateIndex]} is not decimal");

        return new ExchangeRate(DefaultCurrency, currency, rate);
    }
}