namespace Mews.BackendDeveloperTask.ExchangeRates;

public interface IExchangeRateDataSource
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync();
}
