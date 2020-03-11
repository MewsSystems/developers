using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IRatesProvider
    {
        IEnumerable<ExchangeRate> GetAllRates();
    }
}