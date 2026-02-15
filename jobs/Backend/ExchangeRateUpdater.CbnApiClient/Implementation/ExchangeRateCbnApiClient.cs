using ExchangeRateUpdater.Abstraction.Interfaces;
using ExchangeRateUpdater.Abstraction.Model;
using ExchangeRateUpdater.CbnApiClient.DTO;
using ExchangeRateUpdater.CbnApiClient.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ExchangeRateUpdater.CbnApiClient.Implementation
{
    public class ExchangeRateCbnApiClient(IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<ExchangeRateCbnApiClient> logger) : IExchangeRateClient
    {
        private readonly string CnbApiUrl = config.GetValue<string>("CbnApiUrl") ?? throw new InvalidOperationException("CbnApiUrl configuration is missing.");
        // Supports EN and CZ, CZ is the default if not specified.
        private readonly string CnbLangToRequest = "EN";
        private readonly string CbnApiClientName = "CbnApi";

        /// <summary>
        /// Fetches exchange rates from the CBN API for the specified date. If no date is provided, it fetches today's rates.
        /// </summary>
        /// <param name="validDate">The date for which to fetch exchange rates. If null, today's rates will be fetched.</param>
        /// <returns>A collection of <see cref="ExchangeRate"/> objects representing the exchange rates for the specified date.</returns>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request to the CBN API fails.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the response from the CBN API is empty or cannot be deserialized.</exception>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime? validDate = null)
        {
            // prepare the url, add today date as yyyy-MM-dd if specified and lang to "EN"
            string urlWithParams = $"{CnbApiUrl}?lang={CnbLangToRequest}";
            // if no date is specified, the CBN API will return today as default
            if (validDate.HasValue)
            {
                urlWithParams += $"&date={validDate.Value:yyyy-MM-dd}";
            }
            // make the request to the CBN API using the named client so configured handlers/policies are applied
            HttpClient httpClient = httpClientFactory.CreateClient(CbnApiClientName);
            using HttpResponseMessage response = await httpClient.GetAsync(urlWithParams);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch exchange rates from CBN API. Status code: {response.StatusCode}");
            }

            // check content is not empty
            string content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
            {
                string message = "Received empty response from CBN API.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            // deserialize the response. JsonSerializer.Deserialize may throw JsonException for invalid JSON;
            ExchangeRatesResponse? dtoRates;
            try
            {
                dtoRates = JsonSerializer.Deserialize<ExchangeRatesResponse>(content);
            }
            catch (JsonException jex)
            {
                logger.LogError(jex, "Failed to deserialize CBN API response JSON.");
                throw new InvalidOperationException("Failed to deserialize CBN API response.", jex);
            }

            if (dtoRates is null)
                throw new InvalidOperationException("Failed to deserialize CBN API response.");
            return dtoRates.Rates.ToDomain();
        }
    }
}
