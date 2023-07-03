using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.HttpUtils
{
    public interface ICnbClient
    {
        Task<IEnumerable<ExchangeRate>> GetCurrentExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}
