namespace ExchangeRateUpdater.Domain.Interfaces
{
    public interface IHttpClientService
    {
        Task<TResult> GetAsync<TResult, TRequest>(string httpClientName, string uri, TRequest request);
    }
}
