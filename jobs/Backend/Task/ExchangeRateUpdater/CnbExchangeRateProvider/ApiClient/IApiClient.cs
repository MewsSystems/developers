using System.Threading.Tasks;
using ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient.Models;

namespace ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient;

public interface IApiClient
{
    Task<DailyExchangeRateApiModel?> GetDailyExchangeRatesAsync();
}