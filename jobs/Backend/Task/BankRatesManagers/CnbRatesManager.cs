using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.BankRatesManagers;

/// <inheritdoc/>
public class CnbRatesManager: IBankRatesManager
{
    private const int DailyExchangeRateColumnCount = 5;
    private const string SourceCurrency = "CZ";
    private readonly Uri dailyDataSourceUri = new("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");

    /// <inheritdoc/>
    public IEnumerable<ExchangeRate> Parse(string input)
    {
        return input
            .Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Skip(2) // first line is date, second line is header
            .Select(line => ParseLine(line));
    }

    /// <inheritdoc/>
    public ExchangeRate ParseLine(string line)
    {
        var cols = line.Split('|');

        if (cols.Length != DailyExchangeRateColumnCount)
        {
            throw new ArgumentException($"Line has wrong column count. Expected {DailyExchangeRateColumnCount}. " +
                $"But given {cols.Length}. Cols: {cols}");
        }

        if (!decimal.TryParse(cols[4], out decimal rate))
        {
            throw new ArgumentException($"Cannot parse rate from given line: {cols}");
        }

        if (!decimal.TryParse(cols[2], out decimal amount))
        {
            throw new ArgumentException($"Cannot parse amount from given line: {cols}");
        }

        return new ExchangeRate(new Currency(SourceCurrency), new Currency(cols[3]), rate / amount);
    }

    /// <inheritdoc/>
    public Uri GetDailyDataSourceUri()
    {
        return dailyDataSourceUri;
    }
}

