namespace ExchangeRateUpdater;

public interface ICnbExchangeRateService
{
    Task<Dictionary<string, decimal>> GetDailyRatesAsync(DateTime? date = null, CancellationToken cancellationToken = default);
}
