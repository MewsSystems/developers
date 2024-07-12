namespace ExchangeRateUpdate.Core.Interfaces;

public interface IApiHttpClient
{
    Task<IEnumerable<CNBApiExchangeRateRecord>> GetDailyExchangeRatesAsync();
}