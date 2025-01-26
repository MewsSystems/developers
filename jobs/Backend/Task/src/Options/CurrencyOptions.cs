using System.Collections.Generic;

namespace ExchangeRateUpdater.Options
{
    public class CurrencyOptions
    {
        public List<string> IsoCurrencyCodes { get; set; } = new List<string>();
        public string IsoCodesFilePath { get; set; }
    }
} 