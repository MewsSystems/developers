using ExchangeRateUpdater.Business.ExchangeRates.Extensions;
using ExchangeRateUpdater.Dto.ExchangeRates;
using ExchangeRateUpdater.Infrastructure.CNB;
using ExchangeRateUpdater.Infrastructure.CNB.Entities;

namespace ExchangeRateUpdater.Business.ExchangeRates;

public class ExchangeRateProvider(ICnbProvider cnbProvider)
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<List<ExchangeRate>> GetExchangeRates(List<Currency> currencies)
    {
        if (currencies.Count == 0)
        {
            return [];
        }

        CnbExchangeResponseEntity cnbExchangeResponseEntity = await cnbProvider.GetLatestExchangeInformation();

        return cnbExchangeResponseEntity.Rates
            .Where(cnbExchangeRateDto => currencies
                .Any(currency =>
                    currency.Code.Equals(cnbExchangeRateDto.CurrencyCode, StringComparison.CurrentCultureIgnoreCase)))
            .Select(cnbExchangeRateDto => cnbExchangeRateDto.ToExchangeRate())
            .ToList();
    }
}