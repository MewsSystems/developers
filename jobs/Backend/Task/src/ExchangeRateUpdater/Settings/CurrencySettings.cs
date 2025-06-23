using System.Collections.Generic;

namespace ExchangeRateUpdater.Settings
{
    public class CurrencySettings
    {
        public IEnumerable<string> SupportedCurrencies { get; set; }
    }
}
