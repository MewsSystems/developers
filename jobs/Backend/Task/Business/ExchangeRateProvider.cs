using ExchangeRateUpdater.Business.Models;
using ExchangeRateUpdater.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Business
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private const string DEFAULT_CURRENCY = "CZK";

        // This list represents the full list of exchange rates retrieved daily by calling the CNB API.
        // Further improvement would be to keep track of the last time the API was called to see if it's time to update the database.
        private static List<ExchangeRate> __ExchangeRates = new();

        private readonly IExchangeRateService __ExchangeRateService;
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public ExchangeRateProvider()
            : this(new ExchangeRateService())
        { }

        public ExchangeRateProvider(IExchangeRateService exchangeRateService)
        {
            __ExchangeRateService = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));
        }

        public async Task<List<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (__ExchangeRates.Count == 0)
            {
                List<ExchangeRateModel> _Results = await __ExchangeRateService.GetExchangeRatesAsync();
                if (_Results != null && _Results.Any())
                {
                    __ExchangeRates = _Results.Select(result => new ExchangeRate(new Currency(DEFAULT_CURRENCY), new Currency(result.CurrencyCode), result.Rate)).ToList();
                }
            }

            if (currencies != null && currencies.Any())
            {
                return __ExchangeRates.Where(result => currencies.Any(currency => currency.Code == result.TargetCurrency.Code)).ToList();
            }

            return __ExchangeRates;
        }
    }
}
