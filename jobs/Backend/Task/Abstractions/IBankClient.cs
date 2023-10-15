using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IBankClient
    {
        Task<GetExchangeRatesDto> GetExchangeRatesAsync();
    }
}
