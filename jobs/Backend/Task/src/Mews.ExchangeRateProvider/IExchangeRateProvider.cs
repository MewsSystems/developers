namespace Mews.ExchangeRateProvider;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
}