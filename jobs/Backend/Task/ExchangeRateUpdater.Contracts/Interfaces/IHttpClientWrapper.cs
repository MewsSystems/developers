namespace ExchangeRateUpdater.Contracts.Interfaces;

public interface IHttpClientWrapper
{
    Task<HttpResponseMessage> GetAsync(string url);
}
