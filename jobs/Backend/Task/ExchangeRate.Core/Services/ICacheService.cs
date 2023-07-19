namespace ExchangeRate.Core.Services;

public interface ICacheService
{
    Task<T> GetAsync<T>(string cacheKey);

    Task<bool> SetAsync<T>(string cacheKey, T value);
}
