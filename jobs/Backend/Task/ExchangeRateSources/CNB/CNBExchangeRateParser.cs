using ExchangeRateUpdater.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater.CNB;

public static partial class CNBExchangeRateParser
{
    public static IEnumerable<ExchangeRate> ParseRates(string input)
    {
        var regex = CNBExchangeRateRegex();
        //Cast to avoid warning about MatchCollection item casted from object to Match
        var matches = regex.Matches(input).Cast<Match>();
        foreach (Match match in matches)
        {
            var sourceCurrency = new Currency(match.Groups["code"].Value);
            var targetCurrency = new Currency("CZK");
            decimal value = decimal.Parse(match.Groups["value"].Value, CultureInfo.GetCultureInfo("cs-CZ"));
            yield return new ExchangeRate(sourceCurrency, targetCurrency, value);
        }
    }

    [GeneratedRegex("^.*\\|(?<code>[a-zA-Z]*)\\|(?<value>\\d*,\\d*)$", RegexOptions.Multiline)]
    private static partial Regex CNBExchangeRateRegex();
}
