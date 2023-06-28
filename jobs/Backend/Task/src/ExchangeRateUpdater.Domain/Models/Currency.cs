namespace ExchangeRateUpdater.Domain.Models;

public sealed record Currency(string Code)
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; } = Code;

    public override string ToString() => Code;

    public override int GetHashCode() => Code.GetHashCode();

    public bool Equals(Currency? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Code == other.Code;
    }
}

