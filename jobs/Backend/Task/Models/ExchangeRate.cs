using System;

namespace ExchangeRateUpdater.Models;

/// <summary>
///     Represents an immutable exchange rate between two currencies for a specific date.
/// </summary>
public class ExchangeRate
{
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, DateOnly date, decimal value)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Date = date;
        Value = value;
    }

    public Currency SourceCurrency { get; }

    public Currency TargetCurrency { get; }

    public decimal Value { get; }

    public DateOnly Date { get; }

    public override string ToString()
    {
        return $"({Date.ToShortDateString()}) {SourceCurrency}/{TargetCurrency}={Value}";
    }
}