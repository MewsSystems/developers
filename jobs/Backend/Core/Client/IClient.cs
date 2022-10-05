using System.Threading.Tasks;

namespace ExchangeRateUpdater.Client
{
    public interface IClient
    {
        Task<IEnumerable<ExchangeRateItem>> GetExchangeRates();
    }
}
