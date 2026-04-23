namespace ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Models;

/// <summary>
/// Represents a single exchange rate record from the CNB daily file.
/// </summary>
/// <param name="Country">Country name.</param>
/// <param name="CurrencyName">Currency name.</param>
/// <param name="Amount">The amount of foreign currency (typically 1, 100, or 1000).</param>
/// <param name="Code">Three-letter ISO 4217 currency code.</param>
/// <param name="Rate">Exchange rate to CZK for the specified amount.</param>
public record ExchangeRate(
    string Country,
    string CurrencyName,
    int Amount,
    string Code,
    decimal Rate
);