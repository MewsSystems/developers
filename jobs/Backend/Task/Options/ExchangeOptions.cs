using System.Collections.Generic;

namespace ExchangeRateUpdater.Options;

public sealed class ExchangeOptions
{
    public string CnbRatesUrl { get; set; }
    
    public string TargetCurrencyCode { get; set; }

    public HashSet<string> RequiredCurrencyCodes { get; set; } = new();
}