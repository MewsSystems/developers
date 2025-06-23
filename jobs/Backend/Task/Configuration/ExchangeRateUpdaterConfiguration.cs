using System.Collections.Generic;

namespace ExchangeRateUpdater.Configuration
{
    public class ExchangeRateUpdaterConfiguration
    {
        public string CNBExchangeProviderUrl { get; set; }

        public IEnumerable<string> TargetCurrencies { get; set; }
    }
}
