namespace Services;

public class Currency : IComparable<Currency>, IEquatable<Currency>
{
    public static Currency Czk { get; } = new("CZK");

    public Currency(string code)
    {
        // Basic validation for the currency code
        // A more robust way would be to use System.Globalization.RegionInfo.ISOCurrencySymbol from all cultures.
        // This requires a different implementation, such as a static hash set of known currencies.
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Currency code cannot be null or empty", nameof(code));
        }
        if (code.Length != 3)
        {
            throw new ArgumentException("Currency code must be exactly 3 characters", nameof(code));
        }
        if (!code.All(char.IsLetter))
        {
            throw new ArgumentException("Currency code must contain only letters", nameof(code));
        }
        Code = code.ToUpperInvariant();
    }

    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; }

    public override string ToString() => Code;

    public int CompareTo(Currency? other)
    {
        if (other is null)
        {
            return 1;
        }
        if (ReferenceEquals(this, other))
        {
            return 0;
        }
        return string.Compare(Code, other.Code, StringComparison.Ordinal);
    }

    public bool Equals(Currency? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return Code == other.Code;
    }

    public override bool Equals(object? obj) => obj is Currency other && Equals(other);

    public override int GetHashCode() => Code.GetHashCode();
}
