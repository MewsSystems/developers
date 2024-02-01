namespace Mews.ExchangeRateProvider;

/// <summary>
/// Represents a monetary currency
/// </summary>
/// <param name="Code">Three-letter ISO 4217 code of the currency</param>
public record Currency(string Code)
{
    /// <summary>
    /// The string representation of this object
    /// </summary>
    /// <returns>Three-letter ISO 4217 code of the currency</returns>
    public override string ToString() => Code;
}