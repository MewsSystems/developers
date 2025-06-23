using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, DateTime date, CancellationToken cancellationToken);
    }
}
