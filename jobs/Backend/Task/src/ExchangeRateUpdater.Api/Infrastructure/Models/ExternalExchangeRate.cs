namespace ExchangeRateUpdater.Api.Infrastructure.Models;

/// <summary>
/// Represents a raw exchange rate record from an external data source.
/// </summary>
/// <param name="SourceCurrencyCode">ISO 4217 currency code of the source currency.</param>
/// <param name="TargetCurrencyCode">ISO 4217 currency code of the target currency.</param>
/// <param name="Rate">Exchange rate value.</param>
/// <param name="Amount">The amount of source currency units the rate applies to (for normalization).</param>
public sealed record ExternalExchangeRate(
    string SourceCurrencyCode,
    string TargetCurrencyCode,
    decimal Rate,
    int Amount = 1);
