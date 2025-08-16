using ExchangeRateUpdater.Domain.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.ExchangeProvider
{
    public interface IExchangeRateProviderService
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
