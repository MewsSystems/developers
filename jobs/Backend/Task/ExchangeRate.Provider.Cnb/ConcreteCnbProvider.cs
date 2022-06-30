using ExchangeRate.Models;
using ExchangeRate.Provider.Base.Interfaces;
using ExchangeRate.Provider.Cnb.Interfaces;
using ExchangeRateDomain = ExchangeRate.Models.ExchangeRate;

namespace ExchangeRate.Provider.Cnb;

public class ConcreteCnbProvider : IExchangeRateProvider
{
    #region Fields

    private readonly ICnbHttpClient _cnbHttpClient;
    private readonly Currency _currency;

    #endregion

    #region Constructors

    public ConcreteCnbProvider(ICnbHttpClient cnbHttpClient, Currency currency)
    {
        _cnbHttpClient = cnbHttpClient;
        _currency = currency;
    }

    #endregion

    /// <summary>
    ///     Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    ///     by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    ///     do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    ///     some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRateDomain>> GetExchangeRates()
    {
        var response = await _cnbHttpClient.GetExchangeRate();

        return from exchangeRate in response
               select new ExchangeRateDomain(new Currency(exchangeRate.Code), _currency, exchangeRate.Rate.Value / exchangeRate.Amount.Value);
    }
}