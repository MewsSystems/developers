namespace ExchangeRateUpdater.WebApi.Services.ExchangeRateParser;

public interface IExchangeRateParser
{
    Task<IEnumerable<ExchangeRate>> ParseExchangeRates();
}