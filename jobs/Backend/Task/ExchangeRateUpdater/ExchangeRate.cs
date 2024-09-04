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

    private bool Equals(ExchangeRate other)
    {
        return SourceCurrency == other.SourceCurrency && TargetCurrency == other.TargetCurrency && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ExchangeRate)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SourceCurrency, TargetCurrency, Value);
    }
}