using System;

namespace ExchangeRateUpdater.model;

public class ExchangeRate
{
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, DateTime date, decimal value)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Date = date;
        Value = value;
    }

    private Currency SourceCurrency { get; }

    private Currency TargetCurrency { get; }

    private decimal Value { get; }

    private DateTime Date { get; }

    public override string ToString()
    {
        return $"({Date.ToShortDateString()}) {SourceCurrency}/{TargetCurrency}={Value}";
    }
}