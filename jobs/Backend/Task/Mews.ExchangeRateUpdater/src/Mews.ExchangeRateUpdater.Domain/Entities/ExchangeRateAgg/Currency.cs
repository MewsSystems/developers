namespace Mews.ExchangeRateUpdater.Domain.Entities.ExchangeRateAgg;

/// <summary>
/// Represents a specific currency identified by its ISO 4217 code.
/// </summary>
public class Currency : IEquatable<Currency>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="code">The three-letter ISO 4217 code representing the currency.</param>
    public Currency(string code)
    {
        Code = code;
    }

    /// <summary>
    /// Gets the three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Returns the ISO 4217 code representing the current currency.
    /// </summary>
    /// <returns>The three-letter ISO 4217 code of the currency.</returns>
    public override string ToString()
    {
        return Code;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current currency.
    /// </summary>
    /// <param name="obj">The object to compare with the current currency.</param>
    /// <returns>true if the specified object is an instance of <see cref="Currency"/> and equal to the current currency; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as Currency);
    }

    /// <summary>
    /// Determines whether another instance of <see cref="Currency"/> is equal to the current instance.
    /// </summary>
    /// <param name="other">The other <see cref="Currency"/> to compare with the current instance.</param>
    /// <returns>true if the specified <see cref="Currency"/> is equal to the current instance; otherwise, false.</returns>
    public bool Equals(Currency? other)
    {
        return other != null && Code == other.Code;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Currency"/>.</returns>
    public override int GetHashCode() => Code.GetHashCode();
}