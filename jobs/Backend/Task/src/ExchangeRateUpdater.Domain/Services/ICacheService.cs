namespace ExchangeRateUpdater.Domain.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key) where T : class;
    Task SetAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration, TimeSpan? absoluteExpirationRelativeToNow,TimeSpan? slidingExpiration) where T : class;
    Task RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);
} 