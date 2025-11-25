namespace ExchangeRateUpdater.Domain.Entities;

using Common;
using ValueObjects;

public class ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
{
    public Currency SourceCurrency { get; set; } = sourceCurrency;

    public Currency TargetCurrency { get; set; } = targetCurrency;

    public decimal Value { get; } = value;

    public static ExchangeRate New(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        Ensure.Argument.NotNull(sourceCurrency, "Source currency should not be null");
        Ensure.Argument.NotNull(targetCurrency,  "Target currency should not be null");
        Ensure.Argument.NotNull(value, "Exchange value should not be null");
        return new ExchangeRate(sourceCurrency, targetCurrency, value);
    }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}