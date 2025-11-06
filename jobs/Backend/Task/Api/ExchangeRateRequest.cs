namespace ExchangeRateUpdater.Api;

/// <summary>
/// Request model for fetching exchange rates
/// </summary>
public record ExchangeRateRequest
{
    /// <summary>
    /// List of currency codes to fetch rates for (e.g., ["USD", "EUR", "GBP"])
    /// </summary>
    public required string[] CurrencyCodes { get; init; }
}
