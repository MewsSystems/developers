using ExchangeRateUpdater.Core.Exceptions;
using ExchangeRateUpdater.Core.Models.CzechNationalBank;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.CzechNationalBank.Sources
{
    /// <summary>
    /// API Swagger: https://api.cnb.cz/cnbapi/swagger-ui.html
    /// Ref: https://developers.cnb.cz/discuss/633c52c8da0e79003c33a2c1
    /// </summary>
    public class CzechNationalBankApi : ICzechNationalBankSource
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<CzechNationalBankApi> _logger;

        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private const string GetExchangeRatesEndpoint = "cnbapi/exrates/daily";

        public CzechNationalBankApi(
            IHttpClientFactory clientFactory,
            ILogger<CzechNationalBankApi> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
            {
                new JsonStringEnumConverter()
            }
            };
        }

        public async Task<ExchangeRatesDailyDto?> GetExchangeRatesAsync()
        {
            using var client = _clientFactory.CreateClient(CzechNationalBankAdapter.ApiClientName);

            var response = await client.GetAsync(GetExchangeRatesUri());

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response?.ReasonPhrase != null ? response.ReasonPhrase : "Unknown reason";
                _logger.LogError(errorMessage, "Could not retrieve data from the Czech National Bank Api");
                throw new CzechNationalBankApiException(errorMessage);
            }

            try
            {
                var jsonText = await response.Content.ReadAsStreamAsync();
                var exchangeRatesDailyDto =
                    await JsonSerializer.DeserializeAsync<ExchangeRatesDailyDto>(jsonText, _jsonSerializerOptions);
                return exchangeRatesDailyDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed parsing response");
                throw new CzechNationalBankApiException("Failed parsing response");
            }
        }

        private static string GetExchangeRatesUri()
        {
            var builder = new UriBuilder(GetExchangeRatesEndpoint);
            builder.Query = $"date={DateTime.UtcNow.ToString("yyyy-MM-dd")}";

            return $"{builder.Uri.Host}{builder.Uri.PathAndQuery}";
        }
    }
}
