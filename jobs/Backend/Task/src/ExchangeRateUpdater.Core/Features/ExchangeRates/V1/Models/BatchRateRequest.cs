namespace ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;

public record BatchRateRequest
{
    public string? Date { get; init; }
    public IEnumerable<string> CurrencyPairs { get; init; } = new List<string>();
}