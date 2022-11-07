namespace ExchangeRateUpdater.Domain;

public class ExchangeRate
{
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, int count, decimal value)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Count = count;
        Value = value;
    }

    public Currency SourceCurrency { get; }

    public Currency TargetCurrency { get; }

    public int Count { get; }

    public decimal Value { get; }

    public override string ToString()
    {
        return $"{Count} {SourceCurrency}/{TargetCurrency}={Value}";
    }
}