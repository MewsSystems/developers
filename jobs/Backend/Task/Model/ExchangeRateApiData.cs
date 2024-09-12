using System.Collections.Generic;

namespace ExchangeRateUpdater.Model;

public class ExchangeRateApiData
{
    public IEnumerable<ExchangeRateItem> Rates { get; set; }
}
