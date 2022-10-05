namespace ExchangeRateUpdater.Client
{
    public interface IClient<T>
    {
        Task<IEnumerable<T>> GetExchangeRates();
    }
}
