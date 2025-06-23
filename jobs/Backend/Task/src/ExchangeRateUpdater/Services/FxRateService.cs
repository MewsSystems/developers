using ExchangeRateUpdater.Models;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services
{
    public class CNBFxRateService : IFxRateService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CNBFxRateService> _logger;

        public CNBFxRateService(IHttpClientFactory httpClientFactory, ILogger<CNBFxRateService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<FxRate>> GetFxRatesAsync(DateTime date, string language, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("CNBExchangeRatesApi");
            var formattedDate = date.ToString("yyyy-MM-dd");

            _logger.LogInformation($"Retrieving exchange rates for {formattedDate}");

            var response = await httpClient.GetAsync($"exrates/daily?date={formattedDate}&lang={language}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"The rates could not be retrieved from the CNB exchange rates API. Status code: {response.StatusCode}");
                return new List<FxRate>();
            }

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = await JsonSerializer.DeserializeAsync<ExchangeApiResponse>(stream, jsonOptions, cancellationToken: cancellationToken);

            return apiResponse.Rates;
        }
    }
}
