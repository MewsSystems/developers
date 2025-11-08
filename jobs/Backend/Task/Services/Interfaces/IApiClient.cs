namespace ExchangeRateUpdater.Services.Interfaces
{
    public interface IApiClient<T>
    {
        Task<IEnumerable<T>> GetExchangeRatesAsync();
    }
}
