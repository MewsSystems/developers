using ExchangeRateUpdater.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers;

internal interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetDailyExchangeRateAsync(IEnumerable<Currency> currencies, DateTime? date = null);
}
