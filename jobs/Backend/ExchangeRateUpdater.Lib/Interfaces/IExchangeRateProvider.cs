namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates();
    public Task<string> GetExchangeRatesXml();
}