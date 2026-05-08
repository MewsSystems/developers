namespace Mews.ExchangeRateMonitor.ExchangeRate.Features.Options;

public sealed record ExchangeRateModuleOptions
{
    public CnbExratesOptions CnbExratesOptions { get; init; } = new();
}
