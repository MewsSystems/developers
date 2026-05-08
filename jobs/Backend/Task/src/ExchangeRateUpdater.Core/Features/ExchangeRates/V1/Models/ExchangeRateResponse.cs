namespace ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;

public record ExchangeRateResponse
{
    public string SourceCurrency { get; init; } = string.Empty;
    public string TargetCurrency { get; init; } = string.Empty;
    public decimal Rate { get; init; }
    public string Date { get; init; } = string.Empty;
    public string DatePublished { get; init; } = string.Empty;
}