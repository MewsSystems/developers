using ExchangeRateUpdater.Core.Entities;
using ExchangeRateUpdater.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Core.Services;

public class ExchangeRateProvider : IExchangeRateProvider
{
    public ExchangeRateProvider(IExchangeRatesClient exchangeRatesClient, IOptions<SupportedCurrenciesOptions> supportedCurrencies)
    {
        ArgumentNullException.ThrowIfNull(exchangeRatesClient);
        ArgumentNullException.ThrowIfNull(supportedCurrencies);
        
        _exchangeRatesClient = exchangeRatesClient;
        _supportedCurrencies = supportedCurrencies.Value.Currencies;
    }
    
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IList<ExchangeRate>> GetExchangeRatesAsync()
    {
        var cnbExchangeRatesDto = await _exchangeRatesClient.GetTodayExchangeRatesAsync();

        var exchangeRates = cnbExchangeRatesDto.Table.Rows
            .Where(w => _supportedCurrencies.Contains(w.Code))
            .Select(s => new ExchangeRate(
                new Currency(s.Code), new Currency("CZK"), Convert.ToDecimal(s.ExchangeRate) / Convert.ToInt16(s.Quantity)))
            .ToList();

        return exchangeRates;
    }
    
    private readonly IExchangeRatesClient _exchangeRatesClient;
    private readonly IReadOnlyList<string> _supportedCurrencies;
}