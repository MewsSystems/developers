namespace ExchangeRateUpdater.Api;

/// <summary>
/// Response model for exchange rate API endpoints
/// </summary>
public record ExchangeRateResponse
{
    /// <summary>
    /// Source currency code (e.g., "USD", "EUR")
    /// </summary>
    public required string SourceCurrency { get; init; }

    /// <summary>
    /// Target currency code (e.g., "CZK")
    /// </summary>
    public required string TargetCurrency { get; init; }

    /// <summary>
    /// Exchange rate value (how many target currency units per 1 source currency unit)
    /// </summary>
    public required decimal Rate { get; init; }
}
