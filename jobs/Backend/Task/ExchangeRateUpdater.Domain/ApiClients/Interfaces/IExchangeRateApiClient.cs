namespace ExchangeRateUpdater.Domain.ApiClients.Interfaces;

public interface IExchangeRateApiClient
{
    Task<string> GetExchangeRatesXml(CancellationToken cancellationToken);
}