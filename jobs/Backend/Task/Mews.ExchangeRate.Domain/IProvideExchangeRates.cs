namespace Mews.ExchangeRate.Domain;
public interface IProvideExchangeRates
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesForCurrenciesAsync(IEnumerable<Currency> currencies);
}
