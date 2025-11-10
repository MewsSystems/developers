namespace ExchangeRateUpdater.Infrastructure.Config;

internal sealed record AppConfiguration
{
    public required ExchangeRateLoaderConfig CnbExchangeRateLoaderConfig { get; init; }
}
