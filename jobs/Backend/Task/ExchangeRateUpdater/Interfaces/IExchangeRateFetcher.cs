using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateFetcher
    {
        Task<string> FetchDailyExchangeRateData(DateOnly? date);
        Task<string> FetchMonthlyExchangeRateData(DateOnly? date);
    }
}
