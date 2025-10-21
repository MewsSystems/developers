using ExchangeRateUpdater.Services.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Contracts
{
    public interface ICnbApiClient
    {
        Task<IEnumerable<CnbApiRateDto>> GetDailyRatesAsync();
    }
}