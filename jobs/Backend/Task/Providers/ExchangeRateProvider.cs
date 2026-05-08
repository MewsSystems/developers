using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers
{
    public class ExchangeRateProvider
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>

        private async Task<List<ExchangeRateModel>> AccessCurrentExchangeRateFixingCZKAsync()
        {
            var exchangeRateFixingResponse = await _httpClient.GetStringAsync("cnbapi/exrates/daily") ?? throw new Exception("Could not retrieve exchange rate fixing from CNB.");

            ExchangeRateFixingResponse exchangeRateFixing = JsonSerializer.Deserialize<ExchangeRateFixingResponse>(exchangeRateFixingResponse);

            if (exchangeRateFixing?.Rates == null)
                throw new InvalidOperationException("CNB response did not contain 'rates'.");

            List<ExchangeRateModel> exchangeRates = new List<ExchangeRateModel>();

            foreach (var line in exchangeRateFixing.Rates)
            {
                exchangeRates.Add(new ExchangeRateModel(new CurrencyModel(line.CurrencyCode), new CurrencyModel("CZK"), line.Amount > 1 ? line.Rate/line.Amount : line.Rate));
            }

            return exchangeRates;
        }

        public async Task<IEnumerable<ExchangeRateModel>> GetExchangeRatesAsync(IEnumerable<CurrencyModel> currencies)
        {
            List<ExchangeRateModel> exchanges = await AccessCurrentExchangeRateFixingCZKAsync();

            return exchanges.Where(ex => currencies.Any(cur => cur.Code == ex.TargetCurrency.Code) && currencies.Any(cur => cur.Code == ex.SourceCurrency.Code));
        }
    }
}
