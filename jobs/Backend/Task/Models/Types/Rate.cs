using System;

namespace ExchangeRateUpdater.Models.Types;

/// <summary>
/// Represents a rate with a decimal value.
/// </summary>
internal record Rate
{
    public decimal Value { get; init; }

    public Rate(decimal value)
    {
        Value = value;
    }

}