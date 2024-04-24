namespace ExchangeRateUpdater.Domain.Entities;

using Common;

public class ExchangeRate
{
    public ExchangeRate()
    {
    }

    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = value;
    }

    public Currency SourceCurrency { get; set; }

    public Currency TargetCurrency { get; set; }

    public decimal Value { get; }

    // public static ExchangeRate New(Currency sourceCurrency, Currency targetCurrency, decimal value)
    // {
    //     Ensure.Argument.NotNull(sourceCurrency, "Source currency should not be null");
    //     Ensure.Argument.NotNull(targetCurrency,  "Target currency should not be null");
    //     Ensure.Argument.NotNull(value, "Exchange value should not be null");
    //     return new ExchangeRate(sourceCurrency, targetCurrency, value);
    // }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}