using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateProvider(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var exchangeRates = new List<ExchangeRate>();
        var exchangeRatesFromService = await _exchangeRateService.GetExchangeRates();

        foreach (var currency in currencies)
        {
            var exchangeRate = exchangeRatesFromService.Rates.FirstOrDefault(rate => rate.CurrencyCode == currency.Code);
            if (exchangeRate != null)
            {
                var value = exchangeRate.Rate / exchangeRate.Amount;
                exchangeRates.Add(new ExchangeRate(currency, new Currency("CZK"), value));
            }
        }

        return exchangeRates;
    }
}
