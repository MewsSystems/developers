namespace ExchangeRateUpdater.Contracts
{
    public interface IExchangeRateProvider
    {
        Task<List<ExchangeRate>> GetAsync(DateOnly date, CancellationToken ct = default);
    }
}
