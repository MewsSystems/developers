using ExchangeRateUpdater;
using System.Collections.Generic;

public static class ExchangeRateParser
{
    public static List<ExchangeRate> Parse(string content, Currency targetCurrency)
    {
        var exchangeRates = new List<ExchangeRate>();

        var lines = content.Split('\n');

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            var parts = line.Split('|');

            if (parts.Length >= 5)
            {
                var sourceAmount = parts[2];
                var sourceCode = parts[3];
                var sourceRate = parts[4];

                decimal sourceAmountDecimal, sourceRateDecimal;
                if (decimal.TryParse(sourceAmount, out sourceAmountDecimal) && decimal.TryParse(sourceRate, out sourceRateDecimal))
                {
                    var calculatedRate = sourceRateDecimal / sourceAmountDecimal;
                    var exchangeRate = new ExchangeRate(new Currency(sourceCode), new Currency(targetCurrency.Code), calculatedRate);
                    exchangeRates.Add(exchangeRate);
                }
            }
        }

        return exchangeRates;
    }
}
