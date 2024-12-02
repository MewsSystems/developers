using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater;

public class ExchangeRatesConfig
{
    public string? Url { get; set; }
    public HashSet<Currency>? Currencies { get; set; }
}