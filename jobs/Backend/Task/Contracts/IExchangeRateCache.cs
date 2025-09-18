namespace ExchangeRateUpdater.Contracts;

public interface IExchangeRateCache
{
    Task<List<ExchangeRate>?> GetAsync(string key, CancellationToken ct);
    Task SetAsync(string key, List<ExchangeRate> value, CancellationToken ct);
}
