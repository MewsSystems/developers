using ExchangeRateUpdaterModels.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace ExchangeRateProviderService.Services
{
    /// <summary>
    /// Client for retrieving exchange rate data from an external service.
    /// </summary>
    public class DataRetrievalClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _exchangeRetrievalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrievalClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to make requests.</param>
        /// <param name="appSettings">The application settings containing the exchange retrieval service URL.</param>
        public DataRetrievalClient(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _exchangeRetrievalService = appSettings.Value.ExchangeRetrievalService;
        }

        /// <summary>
        /// Asynchronously retrieves exchange rates for the specified currencies.
        /// </summary>
        /// <param name="currencies">The currencies for which to retrieve exchange rates.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of exchange rate models.</returns>
        public async Task<IEnumerable<ExchangeRateModel>> GetExchangeRatesAsync(IEnumerable<CurrencyModel> currencies)
        {
            try
            {
                string url = $"{_exchangeRetrievalService}/api/CNBExchangeRetreival";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                var exchangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateModel>>(content);

                var filteredRates = exchangeRates.Where(item => currencies.Any(c => c.Code == item.SourceCurrency.Code)).ToList();

                return filteredRates;

            }
            catch (Exception e)
            {
                Log.Error(e, "Error fetching exchange rates from CNB API");
                return null;
            }
        }
    }
}
