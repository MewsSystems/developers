using ExchangeRateUpdater.Data;

namespace ExchangeRateUpdater.ExchangeRateSources.CNB;

public sealed class CNBSourceOptions
{
    public SourceLocation Location { get; set; }
    public string DayRateUrl { get; set; } = string.Empty;
    public string FileUri { get; set; } = string.Empty;
}
