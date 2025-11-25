namespace ExchangeRateUpdater.Domain.Entities;

using Common;
using ValueObjects;

public class ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
{
    public Currency SourceCurrency { get; set; } = sourceCurrency;

    public Currency TargetCurrency { get; set; } = targetCurrency;

    public decimal Value { get; } = value;

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}