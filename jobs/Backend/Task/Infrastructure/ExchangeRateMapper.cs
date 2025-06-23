using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExchangeRateUpdater.Infrastructure;

public class ExchangeRateMapper : IExchangeRateMapper
{
    public Result<ExchangeRate[]> Map(ReadOnlySpan<char> data)
    {
        try
        {
            var lines = data.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).AsSpan();

            var maxCount = lines.Length;
            var exchangeRates = new ExchangeRate[maxCount];
            int index = 0;

            for (int i = 2; i < lines.Length; i++)
            {
                var columns = lines[i].Split('|');
                if (columns.Length == 5)
                {
                    var targetCurrency = new Currency(columns[3]);
                    var rate = decimal.Parse(columns[4], CultureInfo.InvariantCulture);

                    exchangeRates[index++] = new ExchangeRate(new Currency("CZK"), targetCurrency, rate);
                }
            }

            if (index > 0)
            {
                var resultArray = new ExchangeRate[index];

                Array.Copy(exchangeRates, resultArray, index);

                return Result<ExchangeRate[]>.Success(resultArray);
            }
            else
            {
                return Result<ExchangeRate[]>.Success(null);
            }
        }
        catch (FormatException e)
        {
            return Result<ExchangeRate[]>.Failure($"Data mapping error: {e.Message}");
        }
    }
}
