using Domain.Entities;

namespace Domain.Ports;

public interface IExchangeRateInMemoryCache
{
    IEnumerable<ExchangeRate>? GetCache(string key);
    void SetCache(string key, object exchangeRates);
}