using Domain.Models;

namespace Domain.Abstractions;

public interface IExchangeRateProvider
{
    Task<List<ExchangeRate>> GetDailyExchangeRates(Currency sourceCurrency, string language);
    Task<ExchangeRate?> GetExchangeRate(Currency source, Currency target, string language = "EN");
}
