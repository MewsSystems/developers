using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Application.Services
{
    public interface IExchangeRateProvider 
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<string> currencies);
    }

    public class ExchangeRateProvider : IExchangeRateProvider
    {
        // NOTES: this is going to be an interesting discussion,
        // how to generalise this client so that the GetExchangeRates method could
        // use any client (if we are to extend this to other banks)
        private readonly ICnbApiClient _cnbApiClient;
        private readonly Currency _targetCurrency;

        public ExchangeRateProvider(ICnbApiClient client)
        {
            _cnbApiClient = client;
            _targetCurrency = new Currency("CZK"); // TODO to come from config?
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<string> currencies)
        {
            var apiResponse = await _cnbApiClient.GetDailyExchangeRates(null, null);

            var exchangeRates = apiResponse.Rates
                .Where(exRate => currencies.Select(c => c).Contains(exRate.CurrencyCode.ToUpper()))
                .Select(exRate => new ExchangeRate(new Currency(exRate.CurrencyCode), _targetCurrency, (exRate.Rate / exRate.Amount)))
                .ToList();

            return exchangeRates;
        }
    }
}
