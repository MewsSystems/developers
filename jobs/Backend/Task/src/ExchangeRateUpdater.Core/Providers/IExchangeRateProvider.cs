namespace ExchangeRateUpdater.Core.Providers
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync();

        IEnumerable<Currency> GetCurrencies();
    }
}
