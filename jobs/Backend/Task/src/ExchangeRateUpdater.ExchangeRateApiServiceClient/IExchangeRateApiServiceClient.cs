using System.Threading.Tasks;
using RestEase;

namespace ExchangeRateUpdater.ExchangeRateApiServiceClient
{

    public interface IExchangeRateApiServiceClient
    {
        [Get("{currency}")]
        Task<GetExchangeRatesResponse> GetExchageRates([Path("currency")] string currencyCode);
    }
}