using ExchangeRateUpdater.CzechNationalBank;
using ExchangeRateUpdater.CzechNationalBank.Contracts;

namespace ExchangeRateUpdater;

internal sealed class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICzechNationalBankClient _client;

    public ExchangeRateProvider(ICzechNationalBankClient client)
    {
        _client = client ??
            throw new ArgumentNullException(nameof(client));
    }

    public Currency TargetCurrency => new("CZK");

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        if (!currencies.Any())
        { 
            return Enumerable.Empty<ExchangeRate>();
        }

        GetDailyExchangeRatesResponse response = await _client.GetDailyExchangeRatesAsync();

        IEnumerable<string> targetCurrencyCodes = currencies
            .Select(x => x.Code)
            .ToList();

        IEnumerable<ExchangeRate> exchangeRates = response.Rates
            .Where(x => targetCurrencyCodes.Contains(x.CurrencyCode))
            .Select(x => new ExchangeRate(sourceCurrency: new Currency(x.CurrencyCode), TargetCurrency, x.Rate / x.Amount))
            .ToList();

        return exchangeRates;
    }
}