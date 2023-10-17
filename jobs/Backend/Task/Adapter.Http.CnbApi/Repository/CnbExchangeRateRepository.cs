using System.ComponentModel.DataAnnotations;
using Adapter.Http.CnbApi.DTO;
using Adapter.Http.CnbApi.Settings;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace Adapter.Http.CnbApi.Repository
{
    public class CnbExchangeRateRepository : IExchangeRateRepository
    {
        private readonly ExchangeRateSettings _exchangeRateSettings;
        private readonly ILogger<CnbExchangeRateRepository> _logger;

        public CnbExchangeRateRepository(
            ExchangeRateSettings appSettings,
            ILogger<CnbExchangeRateRepository> logger)
        {
            _exchangeRateSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(string defaultCurrency, IEnumerable<CurrencyCode> currencies)
        {
            try
            {
                _logger.LogInformation("Start GetExchangeRates.");

                var exRatesDailyUrl = _exchangeRateSettings.ApiUrl.AppendPathSegment("exrates/daily");
                var response = await exRatesDailyUrl.GetJsonAsync<ExchangeRatesResponseDto>();

                _logger.LogInformation($"Success httpcall: {exRatesDailyUrl}");

                if (!response.Rates.Any())
                    throw new ValidationException("No rates found.");

                _logger.LogInformation($"Received {response.Rates.Count} ExchangeRates.");

                var validRates = response.Rates.Where(r => currencies.Any(c => c.Value.ToUpper().Equals(r.CurrencyCode.ToUpper())));

                _logger.LogInformation($"{validRates.Count()} Valid CurrencyCode Exchange.");

                var exchangeRatesList = new List<ExchangeRate>();
                exchangeRatesList.AddRange(validRates.Select(er => new ExchangeRate(
                                        new CurrencyCode(er.CurrencyCode),
                                        new CurrencyCode(defaultCurrency),
                                        Math.Round(er.Rate / er.Amount, 2))));

                _logger.LogInformation($"{exchangeRatesList.Count()} Converted CurrencyCode Exchange.");

                return exchangeRatesList;
            }
            catch (FlurlHttpException)
            {
                throw;
            }
        }
    }
}
