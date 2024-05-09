namespace ExchangeRateUpdater.Core.Providers
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates();
    }
}
