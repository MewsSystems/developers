namespace Mews.BackendDeveloperTask.ExchangeRates.Cnb;

public interface ICnbTextExchangeRateRetriever
{
    Task<string> GetDailyRatesAsync();
}
