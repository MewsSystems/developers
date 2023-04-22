using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Providers
{
    internal interface IExchangeRateProvider
    {
        IAsyncEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
        void PrintExchangeRates(IEnumerable<Currency> currencies);
    }
}
