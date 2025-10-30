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

    public Currency SourceCurrency { get; }

    public Currency TargetCurrency { get; }

    public decimal Value { get; }
    
    public DateTime Date { get; }

    public override string ToString()
    {
        return $"({Date}) {SourceCurrency}/{TargetCurrency}={Value}";
    }
}