using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly string _baseUrl;

        public ExchangeRateProvider(ILogger<ExchangeRateProvider> logger, IConfiguration configuration, HttpClient httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _logger = logger;
            _baseUrl = configuration["ExchangeRateApi:BaseUrl"];
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try
            {
                var exchangeRates = new List<ExchangeRate>();
                var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code));
                string rawData = await FetchExchangeRateDataAsync();

                var response = JsonSerializer.Deserialize<ExchangeRateResponse>(rawData);

                if (response?.Rates != null)
                {
                    foreach (var rate in response.Rates)
                    {
                        if (currencyCodes.Contains(rate.CurrencyCode))
                        {
                            exchangeRates.Add(new ExchangeRate(new Currency("CZK"), new Currency(rate.CurrencyCode), rate.RateValue));
                        }
                    }
                }

                return exchangeRates;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Error fetching data from the API");
                throw;
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error parsing JSON data");
                throw;
            }
        }

        private async Task<string> FetchExchangeRateDataAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                throw new Exception("Error fetching data from Czech National Bank", e);
            }
        }
    }

    public class ExchangeRateResponse
    {
        [JsonPropertyName("rates")] public List<Rate> Rates { get; set; }
    }

    public class Rate
    {
        [JsonPropertyName("currencyCode")] public string CurrencyCode { get; set; }

        [JsonPropertyName("rate")] public decimal RateValue { get; set; }
    }
}
