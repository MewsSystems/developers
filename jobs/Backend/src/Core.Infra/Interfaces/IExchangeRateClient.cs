using RestEase;

namespace Core.Infra.Interfaces
{
    public interface IExchangeRateClient
    {
        [Get("")]
        [AllowAnyStatusCode]
        Task<string> GetExchangeRates();
    }
}
