namespace ExchangeRateUpdater.Contracts.Interfaces;

public interface IExchangeRateProviderService
{
    Task<ExchangeRateResponse> GetExchangeRates(ExchangeRateRequest request);
}
