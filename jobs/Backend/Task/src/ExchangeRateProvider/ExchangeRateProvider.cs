using ExchangeRateProvider.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider;

public class ExchangeRateProvider(IBankApiClient bankApiClient, IMemoryCache memoryCache, ILogger logger): IExchangeRateProvider
{
	private const string CacheKey = "rates";
    private const string TargetCurrencyCode = "CZK";

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
    {
		var rates = await GetBankCurrencyRatesAsync(cancellationToken);

	    return rates
            .Where(r => currencies.Any(c =>
					c.Code == r.CurrencyCode &&
					r.Amount > 0))
            .Select(r =>
                new ExchangeRate(
                    new Currency(r.CurrencyCode),
                    new Currency(TargetCurrencyCode),
                    decimal.Divide(r.Rate, r.Amount)));
    }

    private async Task<IEnumerable<BankCurrencyRate>> GetBankCurrencyRatesAsync(CancellationToken cancellationToken)
    {
	    var rates = memoryCache.Get<IEnumerable<BankCurrencyRate>>(CacheKey);

	    if (rates != null) return rates;

	    try
	    {
		    rates = await bankApiClient.GetDailyExchangeRatesAsync(cancellationToken).ConfigureAwait(false);
	    }
		catch (Exception e)
	    {
		    logger.LogError(e, "Error when retrieving the currency rates from the bank API.");
		    throw;
	    }

		memoryCache.Set(CacheKey, rates, TimeSpan.FromMinutes(60));

	    return rates;
    }
}