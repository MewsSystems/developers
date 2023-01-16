namespace ExchangeRateUpdater.WebApi.Services.ExchangeRateProvider;

public interface IExchangeRateProvider
{
    Task<ServiceResponse<IEnumerable<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency> currencies);
}