using ExchangeRateProvider.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider;

public class ExchangeRateProvider(
	IBankApiClient bankApiClient,
	IMemoryCache memoryCache,
	ILogger<IExchangeRateProvider> logger,
	TimeProvider timeProvider): IExchangeRateProvider
{
	private const string CacheKey = "rates";
	private const double CacheDurationFromNow = 60;
    private const string DefaultTargetCurrencyCode = "CZK";

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTimeOffset? validFor = null, CancellationToken cancellationToken = default)
    {
	    if (validFor != null && validFor.Value > timeProvider.GetUtcNow())
	    {
		    throw new ArgumentException("The validFor parameter value cannot be a date in the future");
	    }

		var rates = await GetBankCurrencyRatesAsync(validFor, cancellationToken);

	    return rates
            .Where(r => currencies.Any(c =>
					c.Code == r.CurrencyCode &&
					r.Amount > 0))
            .Select(r =>
                new ExchangeRate(
                    new Currency(r.CurrencyCode),
                    new Currency(DefaultTargetCurrencyCode),
                    decimal.Divide(r.Rate, r.Amount)));
    }

    private async Task<IEnumerable<BankCurrencyRate>> GetBankCurrencyRatesAsync(DateTimeOffset? validFor, CancellationToken cancellationToken)
    {
	    var cacheKey = GetKey(validFor);
		var rates = memoryCache.Get<IEnumerable<BankCurrencyRate>>(cacheKey);

	    if (rates != null) return rates;

	    try
	    {
		    rates = await bankApiClient.GetDailyExchangeRatesAsync(validFor, cancellationToken).ConfigureAwait(false);
	    }
		catch (Exception e)
	    {
		    logger.LogError(e, "Error when retrieving the currency rates from the bank API.");
		    throw;
	    }

	    var ratesAsArray = rates.ToArray();

		memoryCache.Set(cacheKey, ratesAsArray,
			new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheDurationFromNow),
				Size = ratesAsArray.Length
			});

	    return ratesAsArray;
    }

    private string GetKey(DateTimeOffset? validFor)
    {
	    validFor ??= DateTimeOffset.UtcNow;

	    return $"{CacheKey}-{validFor.Value:yyyy-MM-dd}";
    }
}