using Mews.CurrencyExchange.Domain.Models;

namespace Mews.CurrencyExchange.Providers.Abstractions
{
    public interface ICurrencyExchangeProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> requestedCurrencies);
    }
}
