namespace ExchangeRateUpdater.Domain;

/// <summary>
/// Represents the DTO of an exchange rate between two currencies.
/// </summary>
/// <seealso cref="System.IEquatable&lt;ExchangeRateUpdater.Domain.ExchangeRate&gt;" />
public class ExchangeRate : IEquatable<ExchangeRate>
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

    public override bool Equals(object? obj)
    {
        return Equals(obj as ExchangeRate);
    }

    public bool Equals(ExchangeRate? other)
    {
        return other != null &&
               SourceCurrency.Equals(other.SourceCurrency) &&
               TargetCurrency.Equals(other.TargetCurrency) &&
               Value == other.Value;
    }

    public override int GetHashCode() => HashCode.Combine(SourceCurrency, TargetCurrency, Value);
}