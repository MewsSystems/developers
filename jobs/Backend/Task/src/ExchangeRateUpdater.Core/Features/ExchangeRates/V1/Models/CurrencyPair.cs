namespace ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;

public record CurrencyPair
{
    public string SourceCurrency { get; init; } = string.Empty;
    public string TargetCurrency { get; init; } = string.Empty;
}