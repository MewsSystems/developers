using ExchangeRateUpdater.Models;

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

        public ExchangeRateProvider(
            ICnbApiService cnbApiService
        )
        {
            _cnbApiService = cnbApiService;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // GET CNB RATES
            var cnbRates = await _cnbApiService.GetExchangeRate(cancellationToken);

            var result = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                var rate = cnbRates.Rates.FirstOrDefault(x => x.CurrencyCode.Equals(currency.Code));

                if (rate == null)
                {
                    continue;
                }

                var exchangeRate = new ExchangeRate(new Currency("CZK"), currency, rate.Rate/rate.Amount);

                result.Add(exchangeRate);
            }

            return result;
        }
    }
}
