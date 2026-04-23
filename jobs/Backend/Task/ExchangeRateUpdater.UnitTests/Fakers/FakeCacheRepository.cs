namespace UnitTests.Fakers;

using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Repositories;

public class FakeCacheRepository : ICacheRepository
{
    private readonly Dictionary<string, object?> _store = new();

    public Task<Dictionary<string, ExchangeRate>?> GetRatesDictionaryAsync(string key)
    {
        if (_store.TryGetValue(key, out var v) && v is Dictionary<string, ExchangeRate> t)
            return Task.FromResult<Dictionary<string, ExchangeRate>?>(t);

        return Task.FromResult<Dictionary<string, ExchangeRate>?>(default);
    }

    public Task<List<ExchangeRate>?> GetRatesListAsync(string key)
    {
        if (_store.TryGetValue(key, out var v) && v is List<ExchangeRate> t)
            return Task.FromResult<List<ExchangeRate>?>(t);

        return Task.FromResult<List<ExchangeRate>?>(default);
    }

    public Task SetRatesAsync(string key, Dictionary<string, ExchangeRate> value, TimeSpan expirationDate)
    {
        _store[key] = value!;
        return Task.CompletedTask;
    }

    public Task SetRatesAsync(string key, Dictionary<string, ExchangeRate> value)
    {
        _store[key] = value!;
        return Task.CompletedTask;
    }

    public Task SetRatesListAsync(string key, List<ExchangeRate> value, TimeSpan expirationDate)
    {
        _store[key] = value!;
        return Task.CompletedTask;
    }

    public Task SetRatesListAsync(string key, List<ExchangeRate> value)
    {
        _store[key] = value!;
        return Task.CompletedTask;
    }

    public Task ClearCacheAsync(string key)
    {
        _store.Remove(key);
        return Task.CompletedTask;
    }
}