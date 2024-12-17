namespace ExchangeRateUpdater.Domain;

/// <summary>
/// Represents the a Three-letter ISO 4217 currency code.
/// </summary>
/// <seealso cref="System.IEquatable&lt;ExchangeRateUpdater.Domain.Currency&gt;" />
public class Currency : IEquatable<Currency>
{
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

    public override bool Equals(object? obj)
    {
        return Equals(obj as Currency);
    }

    public bool Equals(Currency? other)
    {
        return other != null && Code == other.Code;
    }

    public override int GetHashCode() => Code.GetHashCode();
}