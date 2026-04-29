using ExchangeRateUpdater.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Business.Services
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRateModel>> GetExchangeRatesAsync();
    }
}