using ExchangeRateUpdater.Models;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
