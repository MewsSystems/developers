using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces;

public interface ICacheService
{
    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan ttl);
}
