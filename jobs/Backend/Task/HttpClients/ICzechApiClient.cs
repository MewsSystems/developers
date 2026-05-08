using System.Threading.Tasks;

namespace ExchangeRateUpdater.HttpClients
{
    public interface ICzechApiClient
    {
        Task<string> GetAsync(string relativeUrl);
    }
}