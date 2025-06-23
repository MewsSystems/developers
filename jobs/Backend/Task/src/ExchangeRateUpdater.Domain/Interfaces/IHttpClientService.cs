using ExchangeRateUpdater.Domain.Ack;

namespace ExchangeRateUpdater.Domain.Interfaces
{
    public interface IHttpClientService
    {
        Task<AckEntity<TResult>> GetAsync<TResult>(string httpClientName, string uri);
    }
}
