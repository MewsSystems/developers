using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateService
    {
        Task ExecuteAsync();
    }
}
