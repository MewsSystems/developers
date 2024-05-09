using ExchangeRateUpdater.Core.Exceptions;
using ExchangeRateUpdater.Core.Models.CzechNationalBank;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.CzechNationalBank.Api
{
    /// <summary>
    /// API Swagger: https://api.cnb.cz/cnbapi/swagger-ui.html
    /// Ref: https://developers.cnb.cz/discuss/633c52c8da0e79003c33a2c1
    /// </summary>
    public class CzechNationalBankApi : ICzechNationalBankApi
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private const string GetExchangeRatesEndpoint = "cnbapi/exrates/daily";

        public CzechNationalBankApi(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
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

        public async Task<ExchangeRatesDailyDto?> GetExchangeRates()
        {
            using var client = _clientFactory.CreateClient(CzechNationalBankAdapter.ApiClientName);

            var response = await client.GetAsync(GetExchangeRatesUri());

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response?.ReasonPhrase != null ? response.ReasonPhrase : "Unknown reason";
                // #TODO: Log
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
                // #TODO: Log
                throw new CzechNationalBankApiException(e.Message);
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
