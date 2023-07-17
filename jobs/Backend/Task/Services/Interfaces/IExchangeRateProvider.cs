using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task GetDataAsync(string url);

        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
