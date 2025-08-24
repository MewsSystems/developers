namespace Mews.ExchangeRateMonitor.ExchangeRate.Features.Options;

public sealed record CnbExratesOptions
{
    public string BaseCnbApiUri { get; init; } = null!;
    public IEnumerable<string> RequiredCurrencies { get; set; } = [];
}
