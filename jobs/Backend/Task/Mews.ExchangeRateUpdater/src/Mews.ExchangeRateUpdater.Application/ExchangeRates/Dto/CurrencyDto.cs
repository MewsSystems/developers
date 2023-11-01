namespace Mews.ExchangeRateUpdater.Application.ExchangeRates.Dto;

/// <summary>
/// Represents the DTO of a currency with its three-letter ISO 4217 code.
/// </summary>
public class CurrencyDto : IEquatable<CurrencyDto>
{
    /// <summary>
    /// Gets or sets the three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// Returns the three-letter ISO 4217 code of the currency as a string.
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
    /// <returns>true if the specified object is equal to the current currency; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as CurrencyDto);
    }

    /// <summary>
    /// Determines whether another instance of <see cref="CurrencyDto"/> is equal to the current instance.
    /// </summary>
    /// <param name="other">The other <see cref="CurrencyDto"/> to compare with the current instance.</param>
    /// <returns>true if the specified <see cref="CurrencyDto"/> is equal to the current instance; otherwise, false.</returns>
    public bool Equals(CurrencyDto? other)
    {
        if (other == null) return false;
        return Code == other.Code;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="CurrencyDto"/>.</returns>
    public override int GetHashCode() => Code.GetHashCode();
}