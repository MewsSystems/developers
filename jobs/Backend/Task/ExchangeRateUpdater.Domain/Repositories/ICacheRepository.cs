namespace ExchangeRateUpdater.Domain.Repositories;

public interface ICacheRepository
{
    T GetFromCache<T>(string key);

    void SetCache<T>(string key, T value, TimeSpan expirationDate);
    
    void SetCache<T>(string key, T value);
        
    void ClearCache(string key);
}