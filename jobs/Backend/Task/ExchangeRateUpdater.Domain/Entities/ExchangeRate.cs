namespace ExchangeRateUpdater.Domain.Entities;

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

    public Currency SourceCurrency { get; init; } = new("CZK");

    public Currency TargetCurrency { get; init; } = new("CZK");

    public decimal Value { get; }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}
