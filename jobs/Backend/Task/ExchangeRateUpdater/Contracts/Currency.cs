using System;

namespace ExchangeRateUpdater.Contracts;

/// <summary>
/// Represents a currency.
/// </summary>
/// <param name="Code">Three-letter ISO 4217 code of the currency.</param>
public readonly record struct Currency(string Code)
{
    public static Currency Czk => new("CZK");

    public bool Equals(Currency other)
    {
        return string.Equals(Code, other.Code, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Code);
    }
}