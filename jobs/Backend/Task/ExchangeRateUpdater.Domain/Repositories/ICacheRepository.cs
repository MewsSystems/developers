namespace ExchangeRateUpdater.Domain.Repositories;

using Domain.Entities;
using System.Collections.Generic;

public interface ICacheRepository
{
    Task<Dictionary<string, ExchangeRate>?> GetRatesDictionaryAsync(string key);

    Task<List<ExchangeRate>?> GetRatesListAsync(string key);

    Task SetRatesAsync(string key, Dictionary<string, ExchangeRate> value, TimeSpan expirationDate);

    Task SetRatesAsync(string key, Dictionary<string, ExchangeRate> value);

    Task SetRatesListAsync(string key, List<ExchangeRate> value, TimeSpan expirationDate);

    Task SetRatesListAsync(string key, List<ExchangeRate> value);

    Task ClearCacheAsync(string key);
}