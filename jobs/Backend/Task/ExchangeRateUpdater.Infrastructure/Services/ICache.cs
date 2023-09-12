namespace ExchangeRateUpdater.Infrastructure.Services
{
    internal interface ICache
    {
        bool Set<T>(string key, T value, DateTimeOffset expiresOn);

        T? Get<T>(string key);
    }
}
