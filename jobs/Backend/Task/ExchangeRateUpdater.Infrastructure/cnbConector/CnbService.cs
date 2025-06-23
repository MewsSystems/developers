using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.Configuration;
using ExchangeRateUpdater.Infrastructure.Dtos;
using ExchangeRateUpdater.Service.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.cnbConector
{
    public class CnbService : ICnbService
    {
        private readonly HttpClient _httpClient;
        private readonly CnbApiOptions _options;
        private readonly ILogger<CnbService> _logger;
        private static readonly Currency czkCurrency = Currency.Create("CZK");

        public CnbService(HttpClient httpClient, IOptions<CnbApiOptions> options, ILogger<CnbService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesByCurrencyAsync(DateTime date, IEnumerable<Currency> currencies)
        {
            var url = $"{_options.BaseUrl}{_options.Methods.Exrates}?date={date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}&lang=EN";

            _logger.LogInformation("Calling cnb api");
            _logger.LogDebug("Request URL: {Url}", url);

            using var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch exchange rates from CNB. StatusCode: {StatusCode}", response.StatusCode);
                return Enumerable.Empty<ExchangeRate>();
            }

            _logger.LogInformation("Cnb api response was successful");

            var json = await response.Content.ReadAsStringAsync();

            var exrateResponse = JsonSerializer.Deserialize<ExrateResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (exrateResponse == null)
                return Enumerable.Empty<ExchangeRate>();

            return exrateResponse.Rates
                .Where(dto => currencies.Any(c => c.Code.Equals(dto.CurrencyCode)))
                .Select(dto =>
                {
                    var targetCurrency = currencies.First(c => c.Code.Equals(dto.CurrencyCode));
                    return ExchangeRate.Create(czkCurrency, targetCurrency, dto.Rate);
                })
                .ToList();
        }
    }
}
