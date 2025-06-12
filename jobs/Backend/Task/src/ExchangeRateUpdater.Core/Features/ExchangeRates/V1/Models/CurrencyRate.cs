namespace ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;

public record CurrencyRate
{
    public string Currency { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public decimal Rate { get; init; }
}