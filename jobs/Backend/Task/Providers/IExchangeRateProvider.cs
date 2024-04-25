using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Providers
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate?>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
