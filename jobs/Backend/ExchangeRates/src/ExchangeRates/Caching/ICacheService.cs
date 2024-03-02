using ExchangeRates.Domain;

namespace ExchangeRates.App.Caching;

public interface ICacheService
{
    public T GetCachedData<T>(string key);
    public void SetCachedData(string key, ExchangeRate value);
}


