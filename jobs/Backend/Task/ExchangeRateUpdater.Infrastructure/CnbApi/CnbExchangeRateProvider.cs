using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Infrastructure.CnbApi;

public class CnbExchangeRateProvider(ICnbApi cnbApi) : IExchangeRateProvider
{
    public async Task<IList<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, DateTime? date = null)
    {
        var response = await cnbApi.GetDailyRates(date?.ToString("yyyy-MM-dd"));
        var rates = new List<ExchangeRate>();

        foreach (var currency in currencies)
        {
            var cnbRate = response.Rates.FirstOrDefault(r => r.CurrencyCode == currency.Code);
            
            if (cnbRate != null)
            {
                // CNB rates are in CZK per unit of foreign currency
                // We need to convert to the requested currency pair
                var rate = new ExchangeRate(
                    new Currency("CZK"),
                    currency,
                    cnbRate.Rate / cnbRate.Amount,
                    DateTime.TryParse(cnbRate.ValidFor, out var dateValid) ? dateValid : null
                );
                rates.Add(rate);
            }
        }

        return rates;
    }
}