
public interface IExchangeRateProvider
{
    Task<ExchangeRate> GetExchangeRate(string currencyCode);
}