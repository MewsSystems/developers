using ExchangeRateUpdater.DTOs;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services;

public interface IExchangeRateService
{
    Task<ExchangeRatesDTO> GetExchangeRates();
}
