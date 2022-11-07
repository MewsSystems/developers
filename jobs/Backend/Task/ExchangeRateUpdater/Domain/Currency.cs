using System;

namespace ExchangeRateUpdater.Domain;

public class Currency : IEquatable<Currency>
{
    public Currency(string code)
    {
        if (string.IsNullOrEmpty(code))
            throw new Exception("Currency code is null or empty");

        if (code.Length != 3)
            throw new Exception($"Currency code: {code} is not is ISO 4217 format");

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
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Currency) obj);
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }
}