using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Adapters
{
    public interface IExchangeRateService
    {
        Task<string> ExchangeRatesFromExternal();
    }
}
