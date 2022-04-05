using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Service
{
    public interface IExchangeRateProvider
    {
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
