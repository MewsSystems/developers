namespace ExchangeRateUpdater.Api.Configuration;

public sealed record ResourcesConfiguration
{
    public string CnbApiUrl { get; init; } = string.Empty;
}