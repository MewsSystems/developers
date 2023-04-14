using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateFetcher
    {
        Task<string> FetchDailyExchangeRateData(DateOnly? date, CancellationToken cancellationToken = default);
        Task<string> FetchMonthlyExchangeRateData(DateOnly? date, CancellationToken cancellationToken = default);
    }
}
