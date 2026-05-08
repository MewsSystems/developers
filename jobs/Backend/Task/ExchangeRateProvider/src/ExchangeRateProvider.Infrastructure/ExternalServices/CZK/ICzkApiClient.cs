namespace ExchangeRateProvider.Infrastructure.ExternalServices.CZK;

public interface ICzkApiClient
{
    Task<CzkExchangeRateResponse?> GetExchangeRatesAsync(CancellationToken cancellationToken = default);
}
