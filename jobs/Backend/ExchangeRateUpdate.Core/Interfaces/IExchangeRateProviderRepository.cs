namespace ExchangeRateUpdate.Core.Interfaces;

public interface IExchangeRateProviderRepository
{
    Task<IEnumerable<CNBApiExchangeRateRecord>> GetDailyExchangeRatesAsync();
}