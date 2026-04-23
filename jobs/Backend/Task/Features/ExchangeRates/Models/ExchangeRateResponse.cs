namespace ExchangeRateUpdater.Features.ExchangeRates.Models;

public sealed class ExchangeRateResponse
{
    public string? ValidFor {  get; init; }
    public int Order {  get; init; }
    public string? Country { get; init; }
    public string? Currency { get; init; }
    public decimal Amount { get; init; }
    public string? CurrencyCode { get; init; }
    public decimal Rate { get; init; }
}
