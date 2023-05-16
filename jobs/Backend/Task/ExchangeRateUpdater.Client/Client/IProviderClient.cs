namespace ExchangeRateUpdater.Client.Client;

public interface IProviderClient
{
    Task<IEnumerable<ExchangeRatePair>> GetAsync(DateTime? date = null);
}