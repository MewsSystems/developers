namespace ExchangeRateUpdater.Infrastructure.Config;

internal record ExchangeRateLoaderConfig
{
    public required string BaseApiUrl { get; init; }

    public RefreshScheduleConfig RefreshScheduleConfig { get; init; }
}
