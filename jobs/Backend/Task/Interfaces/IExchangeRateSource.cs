using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateSource
    {
        public IEnumerable<ExchangeRate> GetAllExchangeRates();
    }
}
