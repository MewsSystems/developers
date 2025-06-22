using ExchangeRateModel;

namespace ExchangeRateService.Provider;

public interface IExchangeRateProvider
{
    Task<ExchangeRate> GetExchangeRate(Currency currency, DateTime date);
    
    Task<IList<ExchangeRate>> GetExchangeRates(IList<Currency> currencies, DateTime date);

}