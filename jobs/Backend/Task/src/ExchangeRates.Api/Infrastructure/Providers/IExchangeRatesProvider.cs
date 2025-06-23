namespace ExchangeRates.Api.Infrastructure.Providers;

public interface IExchangeRatesProvider
{
    Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRatesAsync();
}
