namespace ExchangeRateUpdater;
public class ExchangeRate
{
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = value;
    }

    public Currency SourceCurrency { get; }

    public Currency TargetCurrency { get; }

    public decimal Value { get; }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }

    public bool Equals(ExchangeRate other)
    {
        return SourceCurrency == other.SourceCurrency && TargetCurrency == other.TargetCurrency && Value == other.Value;
    }
}
