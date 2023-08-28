using System.Collections.Generic;

namespace ExchangeRateUpdater.Utilities
{
    public class ServiceConfiguration
    {
        public string CzechNationalBankExchangeApi { get; set; }

        public List<string> Currencies { get; set; }
    }
}
