using System.Collections.Generic;

namespace ExchangeRateUpdater.CnbRates;

public class CnbRatesResult
{
    public IReadOnlyCollection<CnbRateResult> Rates { get; init; } = null!;
}