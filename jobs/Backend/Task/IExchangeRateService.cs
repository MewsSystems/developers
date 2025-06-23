namespace ExchangeRateUpdater;

public interface IExchangeRateService
{
    Task<string?> GetExchangeRatesData();
}
