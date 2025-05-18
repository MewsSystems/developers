namespace ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;

public record BatchExchangeRateResponse
{
    public string Date { get; init; } = string.Empty;
    public string DatePublished { get; init; } = string.Empty;
    public IReadOnlyList<ExchangeRateResponse> Rates { get; init; } = new List<ExchangeRateResponse>();
}