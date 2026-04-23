namespace ExchangeRateUpdater.Domain
{
    public interface IExchangeRateProvider
    {
        Task<IReadOnlyList<ExchangeRate>> GetExchangeRatesAsync(
            IEnumerable<Currency> currencies, string targetCurrencyCode);
    }
}
