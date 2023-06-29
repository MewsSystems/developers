namespace ExchangeRateUpdater.Domain.Models;

/// <summary>
/// Three-letter ISO 4217 code of the currency.
/// </summary>
public sealed record Currency
{
    private readonly string _codeString;

    private Currency(string currencyCode)
    {
        _codeString = currencyCode.ToUpperInvariant();
    }

    public static Currency FromString(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        // TODO: add validation the currency code exists
        return new Currency(value);
    }

    public override string ToString() => _codeString;

    public override int GetHashCode() => _codeString.GetHashCode();

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

        return _codeString == other._codeString;
    }
}

