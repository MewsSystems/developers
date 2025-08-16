using ExchangeRateProvider.Contract.Models;

namespace ExchangeRateProvider.Providers
{
    public interface IExchangeRateProvider
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}
