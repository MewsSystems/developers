using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeRateProviders
{
    public class OtherProvider : IProvider
    {
        // In case if we need some other provider, we can add it here
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            throw new NotImplementedException();
        }
    }
}
