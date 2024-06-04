using System.Collections.Generic;
namespace ExchangeRateProvider.Http;

public interface IExchangeRateClient
{
    Task<IEnumerable<CurrencyRate>> GetDailyExchangeRates();
}