using ExchangeRateProvider.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<ExchangeRateModel[]> GetExchangeRatesAsync(CurrencyModel[] currencies, CancellationToken cancellationToken = default);
    }
}