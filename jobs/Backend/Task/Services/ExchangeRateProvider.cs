using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateProvider
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }

    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateService _exchangeRateService;
        private const string targetCurrency = "CZK";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>

        public ExchangeRateProvider(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRates =  await _exchangeRateService.GetDailyExhangeRate();//call cnb client
            var filteredRates = exchangeRates.Where(rates => currencies.Any(currency => currency.Code == rates.Code));//return matching currencies
            return filteredRates.Select(exchangeRate => new ExchangeRate(new Currency(exchangeRate.Code), new Currency(targetCurrency), exchangeRate.Rate / exchangeRate.Amount));
        }
    }
}
