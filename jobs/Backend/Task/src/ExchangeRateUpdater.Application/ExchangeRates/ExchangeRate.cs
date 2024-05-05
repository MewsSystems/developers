using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Application.ExchangeRates;

public record ExchangeRate
{
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, decimal amount, string validFor)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = value;
        Amount = amount;
        ValidFor = validFor;
    }

    public Currency SourceCurrency { get; }
    public Currency TargetCurrency { get; }

    public decimal Amount { get; }

    public decimal Value { get; }

    public string ValidFor { get; }

    public override string ToString()
    {
        return $"{Amount} {SourceCurrency}/{TargetCurrency}={Value} for date: {ValidFor}";
    }
}
