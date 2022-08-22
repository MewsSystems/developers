using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Parsers;

public class CzechBankCurrencyParser : ICurrencyParser
{
    private readonly ILogger<CzechBankCurrencyParser> _logger;
    private const string CzechBankRateHeader = "Country|Currency|Amount|Code|Rate";
    private readonly int _columnNumber = CzechBankRateHeader.Split('|').Length;

    public CzechBankCurrencyParser(ILogger<CzechBankCurrencyParser> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Parse czech bank currencies from string
    /// </summary>
    /// <param name="data">Czech Bank Currency data in string format</param>
    public IReadOnlyCollection<Currency> ParseCurrencies(string data)
    {
        var currencies = data.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var resultCurrencies = new List<Currency>();
        var headerLine = currencies.Skip(1).FirstOrDefault();

        if (!string.Equals(headerLine, CzechBankRateHeader, StringComparison.InvariantCultureIgnoreCase))
        {
            _logger.LogError($"Rate table header '{headerLine}' is not as expected '{CzechBankRateHeader}'");
            return resultCurrencies;
        }

        foreach (var line in currencies.Skip(2))
        {
            var currency = line.Split('|');

            if (currency.Length != _columnNumber)
            {
                _logger.LogError($"The table row '{line}' does not match to the pattern");
            }

            resultCurrencies.Add(new Currency(
                currency[0],
                currency[1],
                int.Parse(currency[2]),
                currency[3],
                decimal.Parse(currency[4])
            ));
        }

        return resultCurrencies;
    }
}