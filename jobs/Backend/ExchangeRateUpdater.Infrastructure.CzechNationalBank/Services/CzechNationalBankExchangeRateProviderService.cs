using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Models;
using System.Text.Json;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.Services
{
    public class CzechNationalBankExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CzechNationalBankExchangeRateProviderService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var cnbClient = _httpClientFactory.CreateClient("CzechNationalBankApi");
            var todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            var response = await cnbClient.GetAsync($"exrates/daily?date={todayDate}&lang=EN");
            var rawBody = await response.Content.ReadAsStringAsync();
            var exchangeRates = JsonSerializer.Deserialize<DailyRatesResponse>(rawBody);
            var exchangeRatesToReturn = new List<ExchangeRate>();
            if (exchangeRates != null)
            {
                var rates = exchangeRates.Rates.ToDictionary(rate => rate.CurrencyCode, rate => rate.Rate);
                foreach (var currency in currencies)
                {
                    if (rates.TryGetValue(currency.ToString(), out var value))
                    {
                        exchangeRatesToReturn.Add(new ExchangeRate(currency, new Currency("CZK"), value));
                    }
                }
            }
            return exchangeRatesToReturn;
        }
    }
}
