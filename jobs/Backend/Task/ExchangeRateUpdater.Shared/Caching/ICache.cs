namespace ExchangeRateUpdater.Shared.Caching;

public interface ICache
{
    public T GetData<T>(string key);
    public void SetData<T>(string key, T value);
}
