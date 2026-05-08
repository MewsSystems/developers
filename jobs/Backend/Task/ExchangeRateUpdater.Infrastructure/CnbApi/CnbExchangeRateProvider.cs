using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Infrastructure.CnbApi.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.CnbApi;

public class CnbExchangeRateProvider(ICnbApi cnbApi, ILogger<CnbExchangeRateProvider> logger) : IExchangeRateProvider
{
    public async Task<IList<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, DateTime? date = null)
    {
        try
        {
            var response = await cnbApi.GetDailyRates(date?.ToString("yyyy-MM-dd"));
            
            return currencies.Select(currency => CreateExchangeRate(response, currency))
                             .Where(rate => rate is not null)
                             .Select(rate => rate!)
                             .ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching exchange rates from CNB API");
            throw;
        }
    }

    private static ExchangeRate? CreateExchangeRate(CnbExchangeRatesResponse response, Currency currency)
    {
        var cnbRate = response.Rates.FirstOrDefault(r => r.CurrencyCode == currency.Code);
        
        if (cnbRate is null)
        {
            return null;
        }

        // CNB rates are in CZK per unit of foreign currency
        // We need to convert to the requested currency pair
        return new ExchangeRate(
            new Currency("CZK"),
            currency,
            cnbRate.Rate / cnbRate.Amount,
            DateTime.TryParse(cnbRate.ValidFor, out var dateValid) ? dateValid : null
        );
    }
}