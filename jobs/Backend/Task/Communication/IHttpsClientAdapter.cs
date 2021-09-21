using System.Threading.Tasks;

namespace ExchangeRateUpdater.Communication
{
    public interface IHttpsClientAdapter
    {
        Task<string> GetAsync(string url);
    }
}
