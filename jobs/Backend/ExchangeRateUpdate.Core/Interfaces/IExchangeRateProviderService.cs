namespace ExchangeRateUpdate.Core.Interfaces;

public interface IExchangeRateProviderService
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync( IEnumerable<Currency> currencies, bool useCache = false );
}