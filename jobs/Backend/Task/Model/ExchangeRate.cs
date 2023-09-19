using System;

namespace ExchangeRateUpdater.Model;

public class ExchangeRate
{
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, string validFor, decimal value)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        ValidFor = validFor;
        Value = value;
    }

    public Currency SourceCurrency { get; }

    public Currency TargetCurrency { get; }

    public string ValidFor {  get; }

    public decimal Value { get; }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}
