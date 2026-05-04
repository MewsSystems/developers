using System;

namespace ExchangeRateUpdater.Abstractions.Model;

/// <summary>
/// Represents the value of a currency at a specific point in time.
/// </summary>
public class CurrencyValue
{
    /// <summary>
    /// The date for which the exchange rate is valid.
    /// </summary>
    public DateTime ValidFor { get; set; }

    /// <summary>
    /// The amount of the currency unit (e.g., 1 for USD, 100 for JPY).
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// Three-letter ISO 4217 code of the currency (e.g., "USD", "EUR").
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// The exchange rate of the currency relative to a base currency.
    /// </summary>
    public decimal Rate { get; set; }
}