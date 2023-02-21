using System.Collections.Generic;

namespace ExchangeRateUpdater.Data;

public sealed class CurrencyOptions
{
    public IReadOnlyCollection<string> Currencies { get; set; }
}