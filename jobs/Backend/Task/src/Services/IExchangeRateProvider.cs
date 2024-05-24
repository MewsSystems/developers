using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
    }
}