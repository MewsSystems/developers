namespace ExchangeRateUpdater.Infrastructure.Common;

public interface IRestClient
{
    Task<T?> GetAsync<T>(string uri);
}