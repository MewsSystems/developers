namespace Exchange.Application.Abstractions.ApiClients;

public interface ICnbApiClient
{
    Task<IEnumerable<CnbExchangeRate>> GetExchangeRatesAsync(CancellationToken cancellationToken = default);
}