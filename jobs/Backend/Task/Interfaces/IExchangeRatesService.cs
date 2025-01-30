using ExchangeRateUpdater.Models;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRatesService
{
    Task<ExchangeRatesResponseModel> GetExchangeRatesAsync();
}
