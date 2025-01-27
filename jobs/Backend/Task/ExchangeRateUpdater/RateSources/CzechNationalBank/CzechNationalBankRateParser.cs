using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.RateSources.CzechNationalBank;

public interface ICzechNationalBankRateParser
{
    IEnumerable<ExchangeRate> Parse(string rates);
}

class CzechNationalBankRateParser : ICzechNationalBankRateParser
{
    private static Currency CZK = new Currency("CZK");
    private const char NewLine = '\n';
    private const char Separator = '|';
    private const int ExpectedItemsCount = 5; // USA|dollar|1|USD|23.958

    private const int AmountIdx = 2;
    private const int CurrencyIdx = 3;
    private const int ValueIdx = 4;

    public IEnumerable<ExchangeRate> Parse(string rates)
    {
        // First two lines are current date and header names, we don't need them.
        var rateLines = rates.Split('\n').Skip(2);

        foreach (var rawLine in rateLines)
        {
            var line = rawLine.Trim();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            var lineItems = line.Split(Separator).ToList();
            if (lineItems.Count != ExpectedItemsCount)
            {
                throw new CzechNationalBankRateParserException($"Invalid expected items count. [Count = {lineItems.Count}]", line, rates);
            }

            var currency = lineItems[CurrencyIdx];
            var amountStr = lineItems[AmountIdx];
            var rateStr = lineItems[ValueIdx];

            if (!decimal.TryParse(amountStr, out var amount))
            {
                throw new CzechNationalBankRateParserException($"Could not parse amount. [AmountStr = {amountStr}]", line, rates);
            }

            if (!decimal.TryParse(rateStr, out var rate))
            {
                throw new CzechNationalBankRateParserException($"Could not parse rate [RateStr = {rateStr}]", line, rates);
            }


            yield return new ExchangeRate(new(currency), CZK, amount, rate);
        }
    }
}