using ExchangeRateProvider.Models;

namespace ExchangeRateProvider;

public class ExchangeRateProvider(IBankApiClient bankApiClient): IExchangeRateProvider
{
    private const string TargetCurrencyCode = "CZK";

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
    {
	    var rates = await bankApiClient.GetDailyExchangeRatesAsync(cancellationToken).ConfigureAwait(false);
        
        return rates
            .Where(r => currencies.Any(c => c.Code == r.CurrencyCode))
            .Select(r =>
                new ExchangeRate(
                    new Currency(r.CurrencyCode),
                    new Currency(TargetCurrencyCode),
                    decimal.Divide(r.Rate, r.Amount)));
    }
}