using ExchangeRateUpdater.Infrastructure.ExchangeRates;
using ExchangeRateUpdater.Model.ExchangeRates;

namespace ExchangeRateUpdater.Services.ExchangeRates;

public class ExchangeRateProvider(IExchangeRateDataSource exchangeRateDataSource) : IExchangeRateProvider
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        var exchangeRates = await exchangeRateDataSource.GetExchangeRatesAsync(cancellationToken);
        
        return exchangeRates.Where(er =>
        {
            var currenciesList = currencies.ToList();
            return currenciesList.Contains(er.SourceCurrency) && currenciesList.Contains(er.TargetCurrency);
        });
    }
}