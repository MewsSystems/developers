using System.Threading.Tasks;

namespace ExchangeRateUpdater
{

    public interface IExchangeRateService
    {
        Task<string> FetchExchangeRateDataAsync();
    }
}
