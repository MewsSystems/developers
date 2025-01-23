using ExchangeRateUpdater.Domain.Constants;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Models;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.Extensions;

internal static class RateCollectionExtensions
{
    public static IEnumerable<ExchangeRate> FilterAndConvertRates(this IEnumerable<CnbRate>? allRates, IEnumerable<Currency> currencies)
    {
        if (allRates is null) return Enumerable.Empty<ExchangeRate>();
        
        var requestedCurrencyCodes = currencies.Select(x => x.Code.ToLower()).ToHashSet();
        var filteredRates = allRates
            .Where(rate => requestedCurrencyCodes.Contains(rate.CurrencyCode.ToLower()))
            .Select(r=>r.ToExchangeRate(CurrencyCodes.CzechKoruna));

        return filteredRates;
    }
}