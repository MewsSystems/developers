using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateFetcher
    {
        Task<string> FetchExchangeRateData();
    }
}
