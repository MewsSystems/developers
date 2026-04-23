using API.Models;

namespace API.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetCurrentExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
    }
}
