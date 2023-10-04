using Domain.Entities;

namespace Application.ExchangeRateProvider
{
    public interface IExchangeRateProvider
    {

        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);

    }
}
