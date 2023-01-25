using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.AzFunction.Logic.ExchangeRateProvider
{
    public interface IExchangeRateProviderManager
    {
        IExchangeRateProvider GetExchangeRateProvider(string currency);
    }
}
