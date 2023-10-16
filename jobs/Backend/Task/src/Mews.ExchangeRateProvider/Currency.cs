namespace Mews.ExchangeRateProvider;

/// <summary>
/// Represents a monetary currency
/// </summary>
/// <param name="Code">Three-letter ISO 4217 code of the currency.</param>
public sealed record Currency(string Code)
{
    public override string ToString() => Code;
}