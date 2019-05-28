using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateRepository
    {
        IEnumerable<ExchangeRate> GetExchangeRates();
    }
}
