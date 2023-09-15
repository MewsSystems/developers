using Infrastructure.Models.Responses;

namespace Infrastructure.Services.Abstract;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>?> GetExchangeRates(IEnumerable<Currency> currencies);
}
