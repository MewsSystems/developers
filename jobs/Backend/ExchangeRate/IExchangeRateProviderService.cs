using ExchangeRateProvider.Objects;

namespace ExchangeRateProvider
{
    public interface IExchangeRateProviderService
    {
        Task<List<ExchangeRate>> RetrieveExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime date);
    }
}
