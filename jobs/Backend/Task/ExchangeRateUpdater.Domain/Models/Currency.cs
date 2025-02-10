namespace ExchangeRateUpdater.Domain.Models;

/// <summary>
/// Represents a currency using its ISO 4217 code.
/// </summary>
/// <param name="Code">The three-letter currency code defined by ISO 4217.</param>
public sealed record Currency(string Code)
{
    /// <summary>
    /// Returns the ISO 4217 currency code as a string.
    /// </summary>
    /// <returns>The currency code.</returns>
    public override string ToString() => Code;
}
