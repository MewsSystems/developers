namespace ExchangeRateUpdater;

public interface IExchangeRateProvider
{
    Currency TargetCurrency { get; }
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
}
