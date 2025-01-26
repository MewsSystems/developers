using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Interfaces
{
    public interface IExchangeRateDataProvider
    {
        Task<string> GetRawDataAsync();
    }
} 