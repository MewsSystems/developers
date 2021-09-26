using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.ViewModels;

namespace ExchangeRateUpdater.Services
{
    interface IExchangeRateProvider
    {
        Task RetrieveExchangeRatesAsync();
    }
}
