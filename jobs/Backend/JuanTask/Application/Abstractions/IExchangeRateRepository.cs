using Domain.Entities;

namespace Application.Abstractions
{
    public interface IExchangeRateRepository
    {

        Task<IDictionary<string, ExchangeRate>> GetTodayCZKExchangeRatesDictionaryAsync();

    }
}
