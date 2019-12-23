using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class EcbExchangeRateProvider : IExchangeRateProvider
    {
        public ProviderName ProviderName => ProviderName.ECB;

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            throw new NotImplementedException($"{ProviderName} exchange rate provider is not implemented.");
        }
    }
}