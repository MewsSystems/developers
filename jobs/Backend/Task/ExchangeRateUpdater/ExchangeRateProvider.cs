using System.Net.Http.Json;
using ExchangeRateUpdater.Dto;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider : IExchangeRateProvider
{
    // Todo this is maybe not necessary, because we map only values from the API,
    // but it could be good to warn consumer about wrong currency code
    private readonly ICurrencyValidator _currencyValidator;
    private readonly IExchangeRateFetcher _exchangeRateFetcher;

    public ExchangeRateProvider(ICurrencyValidator currencyValidator, IExchangeRateFetcher exchangeRateFetcher)
    {
        _currencyValidator = currencyValidator;
        _exchangeRateFetcher = exchangeRateFetcher;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var invalidCurrencies = currencies
            .Select(x => x.Code)
            .Where(x => !_currencyValidator.IsValid(x));
        
        if (invalidCurrencies.Any())
        {
            throw new ArgumentException($"{string.Join(',', invalidCurrencies)} is not a valid currency code.");
        }
        
        return (await _exchangeRateFetcher.FetchCurrentAsync())?
            .Rates
            .Where(x => currencies.Any(y => y.Code == x.CurrencyCode))
            .Select(x => new ExchangeRate("CZK", x.CurrencyCode, x.Rate / x.Amount))
            ?? Enumerable.Empty<ExchangeRate>();
    }
}