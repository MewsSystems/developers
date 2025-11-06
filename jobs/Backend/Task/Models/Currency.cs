namespace ExchangeRateUpdater.Models;

/// <summary>
/// Represents a currency with its ISO 4217 code.
/// </summary>
/// <param name="Code">Three-letter ISO 4217 code of the currency.</param>
public record Currency(string Code);
