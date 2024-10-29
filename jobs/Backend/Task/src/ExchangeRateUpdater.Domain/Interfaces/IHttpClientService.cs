namespace ExchangeRateUpdater.Domain.Interfaces
{
    public interface IHttpClientService
    {
        Task<TResult> GetAsync<TResult, TRequest>(string uri, TRequest request);
    }
}
