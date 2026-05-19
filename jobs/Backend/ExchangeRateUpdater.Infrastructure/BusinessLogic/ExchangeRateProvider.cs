using ExchangeRateUpdater.Model.Common;
using ExchangeRateUpdater.Model.Configuration;
using ExchangeRateUpdater.Model.Dto;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Json;

namespace ExchangeRateUpdater.Infrastructure.BusinessLogic
{
    public interface IExchangeRateProvider
    {
        Task<ExchangeRateResponseDto> GetExchangeRatesAsync(ExchangeRateRequestDto request);
    }
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>

        private readonly ICnbConfig _config;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProvider(ICnbConfig config, ILogger<ExchangeRateProvider> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<ExchangeRateResponseDto> GetExchangeRatesAsync(ExchangeRateRequestDto request)
        {
            if (request == null || !request.Currencies.Any())
                return new ExchangeRateResponseDto
                {
                    ErrorMessage = "Request can not be null or empty."
                };

            var today = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
            var rates = new List<ExchangeRate>();

            try
            {
                _logger.LogInformation("Exchange rates request initiated");

                using var _client = new HttpClient();
                var taskResp = await _client.GetAsync($"{_config.BaseAddress}/{_config.ApiEndpoint}date={today}");
                taskResp.EnsureSuccessStatusCode();

                var data = await taskResp.Content.ReadFromJsonAsync<RateDataDto>();

                var cnbResult = data.Rates.IntersectBy(request.Currencies.Select(c => c.Code), r => r.CurrencyCode);

                foreach (var rate in cnbResult)
                {
                    rates.Add(new ExchangeRate(new Currency(rate.CurrencyCode),
                        new Currency(_config.TargetCurrency), rate.Rate));
                }
            }
            catch (HttpRequestException ex)
            {
                return new ExchangeRateResponseDto
                {
                    ErrorMessage = $"Http request failed: '{ex.Message}'."
                };
            }
            catch (Exception ex)
            {
                return new ExchangeRateResponseDto
                {
                    ErrorMessage = $"Could not retrieve exchange rates: '{ex.Message}'."
                };
            }

            _logger.LogInformation($"Successfully retrieved {rates.Count} exchange rates:");

            return new ExchangeRateResponseDto { Rates = rates };
        }
    }
}
