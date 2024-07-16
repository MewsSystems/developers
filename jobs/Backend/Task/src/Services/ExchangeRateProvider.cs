using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Options;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>

        private readonly ICnbApiService _cnbApiService;
        private readonly CurrenciesOptions _currenciesOptions;

        public ExchangeRateProvider(
            ICnbApiService cnbApiService,
            IOptionsMonitor<CurrenciesOptions> currenciesOptions
        )
        {
            _cnbApiService = cnbApiService;
            _currenciesOptions = currenciesOptions.CurrentValue;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // GET CNB RATES
            var cnbRates = await _cnbApiService.GetExchangeRate(cancellationToken);

            var result = new List<ExchangeRate>();

            foreach (var currencyCode in _currenciesOptions.Currencies)
            {
                var currencyTarget = new Currency(currencyCode);

                var rate = cnbRates.Rates.FirstOrDefault(x => x.CurrencyCode.Equals(currencyTarget.Code));

                if (rate == null)
                {
                    continue;
                }

                var value = rate.Rate / rate.Amount;

                var exchangeRate = new ExchangeRate(new Currency("CZK"), currencyTarget, value);

                result.Add(exchangeRate);
            }

            return result;
        }
    }
}
