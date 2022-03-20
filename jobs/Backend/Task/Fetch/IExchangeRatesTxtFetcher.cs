using System.Threading.Tasks;

namespace ExchangeRateUpdater.Fetch
{
    public interface IExchangeRatesTxtFetcher
    {
        Task<string> FetchExchangeRates();
    }
}
