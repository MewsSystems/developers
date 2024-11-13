using ExchangeRateUpdaterModels.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.src
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRateModel>> GetExchangeRatesAsync(IEnumerable<CurrencyModel> currencies);
    }
}
