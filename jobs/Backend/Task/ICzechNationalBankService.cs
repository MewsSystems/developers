using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface ICzechNationalBankService
    {
        public IEnumerable<CzechNationalBankExchangeRate> GetRates();
    }
}