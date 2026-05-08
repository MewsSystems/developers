namespace ExchangeRateUpdater.Models;

/// <summary>
/// Data transfer object representing a single exchange rate entry from CNB.
/// </summary>
public record CnbExchangeRateDto
{
    public required string Country { get; init; }
    public required string CurrencyName { get; init; }
    public required int Amount { get; init; }
    public required string Code { get; init; }
    public required decimal Rate { get; init; }
}
