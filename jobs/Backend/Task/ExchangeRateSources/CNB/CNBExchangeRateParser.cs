using ExchangeRateUpdater.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater.CNB;

public static partial class CNBExchangeRateParser
{
    private const string CZECH_CURRENCY = "CZK";
    private const int ERROR_MESSAGE_INPUT_PREVIEW_LENGTH = 50;

    public static IEnumerable<ExchangeRate> ParseRates(string input)
    {
        var regex = CNBExchangeRateRegex();
        //Cast to avoid warning about MatchCollection item casted from object to Match
        var matches = regex.Matches(input).Cast<Match>();
        if (!matches.Any())
        {
            throw new FormatException($"Failed to parse rates. Invalid format: {input.Take(ERROR_MESSAGE_INPUT_PREVIEW_LENGTH)}");
        }
        foreach (Match match in matches)
        {
            ExchangeRate exchangeRate = ComputeExchangeRate(match);
            yield return exchangeRate;
        }

    }

    private static ExchangeRate ComputeExchangeRate(Match match)
    {
        try
        {
            var sourceCurrency = new Currency(match.Groups["code"].Value);
            var targetCurrency = new Currency(CZECH_CURRENCY);
            decimal amount = decimal.Parse(match.Groups["amount"].Value, CultureInfo.GetCultureInfo("cs-CZ"));
            decimal value = decimal.Parse(match.Groups["value"].Value, CultureInfo.GetCultureInfo("cs-CZ"));
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value / amount);
            return exchangeRate;
        }
        catch (Exception e)
        {
            //optionally only log and skip the line
            throw new FormatException($"Failed to parse rates. {match.Value}", e);
        }
    }

    [GeneratedRegex("^.*\\|(?<amount>\\d+)\\|(?<code>[a-zA-Z]*)\\|(?<value>\\d*,\\d*)$", RegexOptions.Multiline)]
    private static partial Regex CNBExchangeRateRegex();
}
