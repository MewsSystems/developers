using Ardalis.GuardClauses;

namespace Mews.ExchangeRate.Domain;
public sealed class ExchangeRate : IEquatable<ExchangeRate>
{
    public Currency SourceCurrency { get; }

    public Currency TargetCurrency { get; }

    public decimal Value { get; }

    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        Guard.Against.Null(sourceCurrency,
            nameof(sourceCurrency));

        Guard.Against.Null(targetCurrency,
            nameof(targetCurrency));

        Guard.Against.NegativeOrZero(value, nameof(value));

        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = value;
    }

    public bool Equals(ExchangeRate? other)
    {
        return other != null && (
            ReferenceEquals(this, other) ||
            other.SourceCurrency.Equals(this.SourceCurrency) &&
            other.TargetCurrency.Equals(this.TargetCurrency) &&
            other.Value.Equals(this.Value));
    }

    public override bool Equals(object? obj)
    {
        return obj is ExchangeRate value &&
            Equals(value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            SourceCurrency.GetHashCode(),
            TargetCurrency.GetHashCode(),
            Value.GetHashCode());
    }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}
