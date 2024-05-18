using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Application.Services
{
    public interface IExchangeRateProvider 
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }

    public class ExchangeRateProvider : IExchangeRateProvider
    {
        // this is going to be an interesting discussion,
        // how to generalise this client so that the GetExchangeRates method could
        // use any client (if we are to extend this to other banks
        private readonly ICnbApiClient _cnbApiClient;
        private readonly Currency _baseCurrency;
        private readonly decimal _roundingDecimal;

        public ExchangeRateProvider(ICnbApiClient client)
        {
            _cnbApiClient = client;
            _baseCurrency = new Currency("CZK"); // TODO to come from config
            _roundingDecimal = 2; // TODO to remove?
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var requestedCurrencyCodes = GetCurrencyCodesAsString(currencies);

            // call external API
            var apiResponse = await _cnbApiClient.GetDailyExchangeRates(null, null);

            var apiExchangeRates = apiResponse.ExchangeRates;

            var exchangeRates = new List<ExchangeRate>();

            foreach (ExchangeRateResponse rate in apiExchangeRates) 
            {
                // if currency code matches with requested curerncy, include it
                if (requestedCurrencyCodes.Contains(rate.CurrencyCode)) 
                {
                    var exchangeRate = new ExchangeRate(_baseCurrency, new Currency(rate.CurrencyCode), rate.Rate);

                    exchangeRates.Add(exchangeRate);
                }
            }

            return exchangeRates;
        }

        private IEnumerable<string> GetCurrencyCodesAsString(IEnumerable<Currency> currencies) 
        {
            var currencyCodes = new List<string>();

            foreach (Currency c in currencies) 
            {
                currencyCodes.Add(c.ToString());
            }

            return currencyCodes;
        }
    }
}
