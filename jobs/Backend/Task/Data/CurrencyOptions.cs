using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Data;

public sealed class CurrencyOptions
{
    public IEnumerable<string> Currencies { get; set; } = Enumerable.Empty<string>();
}