namespace ExchangeRateProvider.Models;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTimeOffset? validFor = null, CancellationToken cancellationToken = default);
}