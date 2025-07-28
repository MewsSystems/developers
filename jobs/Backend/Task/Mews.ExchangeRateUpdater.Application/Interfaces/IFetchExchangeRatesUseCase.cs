namespace Mews.ExchangeRateUpdater.Application.Interfaces;

public interface IFetchExchangeRatesUseCase
{
    Task ExecuteAsync(CancellationToken ct, bool forceUpdate = false);
}