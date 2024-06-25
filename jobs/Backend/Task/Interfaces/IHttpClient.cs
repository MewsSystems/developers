using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string url);
    }
}