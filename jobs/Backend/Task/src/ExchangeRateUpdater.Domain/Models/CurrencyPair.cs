namespace ExchangeRateUpdater.Domain.Models;

public sealed record CurrencyPair(Currency SourceCurrency, Currency TargetCurrency)
{
    public override string ToString() => $"{SourceCurrency}/{TargetCurrency}";

    public override int GetHashCode() => (SourceCurrency, TargetCurrency).GetHashCode();

    public bool Equals(CurrencyPair? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return SourceCurrency == other.SourceCurrency
               && TargetCurrency == other.TargetCurrency;
    }
}
