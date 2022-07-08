using ExchangeRateUpdater.Models;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Helpers.Interfaces
{
    public interface IApiService
    {
        Task<string> GetApiDataAsStringAsync(string uri);
    }
}