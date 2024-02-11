namespace ExchangeRate.Application.Services;

public interface IExchangeRatesService
{
    Task<IEnumerable<Domain.ExchangeRate>> GetCurrentExchangeRates();
}