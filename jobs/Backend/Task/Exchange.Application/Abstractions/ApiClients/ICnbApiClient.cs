namespace Exchange.Application.Abstractions.ApiClients;

public interface ICnbApiClient
{
    Task<IEnumerable<ExchangeRateResponse>> GetExchangeRatesAsync();
}