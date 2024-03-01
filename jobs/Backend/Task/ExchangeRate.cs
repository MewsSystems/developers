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

    public override bool Equals(object obj)
    {
        var other = obj as ExchangeRate;
        if (other == null) return false;
        return SourceCurrency.Equals(other.SourceCurrency) && TargetCurrency.Equals(other.TargetCurrency) &&
               Value == other.Value;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17; // Choose prime numbers as initial values

            // Combine hash codes of properties using XOR (^) operator
            hash = hash * 23 + (SourceCurrency?.GetHashCode() ?? 0);
            hash = hash * 23 + (TargetCurrency?.GetHashCode() ?? 0);
            hash = hash * 23 + Value.GetHashCode();

            return hash;
        }
    }
}