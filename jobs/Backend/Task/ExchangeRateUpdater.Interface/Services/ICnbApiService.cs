using ExchangeRateUpdater.Domain.Model;

namespace ExchangeRateUpdater.Interface.Services
{
    public interface ICnbApiService
    {
        Task<IEnumerable<ExchangeRateEntity>> GetExchangeRates();
    }
}
