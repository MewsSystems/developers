namespace ExchangeRateProvider.Domain.Entities;

/// <summary>
/// Represents a currency with its ISO 4217 code.
/// </summary>
/// <param name="Code">The three-letter ISO 4217 currency code.</param>
public record Currency(string Code)
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; } = Code;

    /// <summary>
    /// Returns the currency code as a string representation.
    /// </summary>
    /// <returns>The ISO 4217 currency code.</returns>
    public override string ToString()
    {
        return Code;
    }
}