namespace ExchangeRateUpdater.Infrastructure.Config;

internal record RefreshScheduleConfig
{
    public required string Time { get; init; }
    public required string TimeZone { get; init; }
}
