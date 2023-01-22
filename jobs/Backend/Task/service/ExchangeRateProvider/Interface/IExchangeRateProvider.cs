using ExchangeRateProvider.Contracts;

namespace ExchangeRateProviderCzechNationalBank.Interface
{
    public interface IExchangeRateProvider
    {
        /// <inheritdoc cref="ExchangeRateProvider.GetExchangeRates(IEnumerable{Currency})"/>
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
