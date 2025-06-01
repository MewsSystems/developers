namespace Services;

public class ExchangeRate : IComparable<ExchangeRate>
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

    public int CompareTo(ExchangeRate? other)
    {
        if (other is null)
        {
            return 1;
        }
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        var compareSource = SourceCurrency.CompareTo(other.SourceCurrency);
        if (compareSource != 0)
        {
            return compareSource;
        }
        return TargetCurrency.CompareTo(other.TargetCurrency);
    }
}