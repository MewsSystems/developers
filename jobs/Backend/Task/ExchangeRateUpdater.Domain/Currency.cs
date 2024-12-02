namespace ExchangeRateUpdater.Domain;

public class Currency : IEquatable<Currency>
{
    public Currency(string code) => Code = code;

    /// <summary>
    ///     Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; }

    public bool Equals(Currency? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return Code == other.Code;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        return Equals((Currency)obj);
    }

    public override int GetHashCode() => Code.GetHashCode();

    public static bool operator ==(Currency? left, Currency? right) => Equals(left, right);

    public static bool operator !=(Currency? left, Currency? right) => !Equals(left, right);

    public override string ToString() => Code;
}