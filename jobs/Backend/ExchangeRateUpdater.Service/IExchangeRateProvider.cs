namespace ExchangeRateUpdater.Service
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, DateTime? referenceDate = null);
    }
}