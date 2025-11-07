using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Services.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class CnbApiClient : ICnbApiClient
    {
        private const string LanguageValue = "CZ";

        private readonly HttpClient _httpClient;
        private readonly CnbApiOptions _options;
        private readonly ILogger<CnbApiClient> _logger;
        private readonly ITimeProvider _timeProvider;

        public CnbApiClient(HttpClient httpClient, IOptions<CnbApiOptions> options, ILogger<CnbApiClient> logger, ITimeProvider timeProvider)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
            _timeProvider = timeProvider;
        }

        public async Task<IEnumerable<CnbApiRateDto>> GetDailyRatesAsync()
        {
            var today = _timeProvider.GetUtcNow().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var queryParams = new Dictionary<string, string> { { "date", today }, { "lang", LanguageValue } };

            var queryString = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            var fullPath = $"{_options.DailyRatesPath}?{queryString}";

            try
            {
                _logger.LogInformation("Querying CNB API for daily rates: {Path}", fullPath);

                var response = await _httpClient.GetFromJsonAsync<CnbApiResponseDto>(fullPath);
                return response?.Rates ?? Enumerable.Empty<CnbApiRateDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while querying CNB API at '{Path}'.", fullPath);
                throw;
            }
        }
    }
}