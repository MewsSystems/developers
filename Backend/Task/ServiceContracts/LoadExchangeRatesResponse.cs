using System.Collections.Generic;

namespace ExchangeRateUpdater.ServiceContracts
{
    public class LoadExchangeRatesResponse
    {
        public IEnumerable<string> Lines { get; set; }
    }
}
