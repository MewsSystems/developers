using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExchangeApis;

namespace ExchangeRateUpdater.RateProvider;

public class ExchangeRateProvider(IExchangeApi api)
{
    private readonly IExchangeApi _api = api;

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
    {
        var rates = await _api.GetAllRates(cancellationToken);

        return from c in currencies
               join r in rates on c.Code equals r.SourceCurrency.Code
               select r;
    }
}
