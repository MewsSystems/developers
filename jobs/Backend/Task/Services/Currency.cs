namespace Services;

public class Currency : IComparable<Currency>, IEquatable<Currency>
{
    public static Currency Czk { get; } = new("CZK");

    public Currency(string code)
    {
        Code = code;
    }

    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; }

    public override string ToString()
    {
        return Code;
    }

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

    public override bool Equals(object? obj)
    {
        return obj is Currency other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }
}