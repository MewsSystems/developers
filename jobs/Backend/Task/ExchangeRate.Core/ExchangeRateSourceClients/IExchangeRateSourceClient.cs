namespace ExchangeRate.Core.ExchangeRateSourceClients;

public interface IExchangeRateSourceClient<T>
{
    Task<IEnumerable<T>> GetExchangeRatesAsync(string urlPath);
}