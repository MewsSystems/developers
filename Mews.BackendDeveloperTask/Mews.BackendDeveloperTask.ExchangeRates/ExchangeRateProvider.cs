namespace Mews.BackendDeveloperTask.ExchangeRates;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateDataSource _dataSource;

    public ExchangeRateProvider(IExchangeRateDataSource dataSource)
    {
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        var requestedCurrencies = currencies.ToHashSet();
        var allExchangeRates = await _dataSource.GetExchangeRatesAsync();
        return allExchangeRates.Where(rate => requestedCurrencies.Contains(rate.Source));
    }
}
