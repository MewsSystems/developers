using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services
{
    public interface ICnbApiService
    {
        Task<CnbRateDailyResponse> GetExchangeRate(CancellationToken cancellationToken);
    }
}