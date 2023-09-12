using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Models.Enums;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Infrastructure.Clients;
using ExchangeRateUpdater.Infrastructure.Models.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.Services;
using ExchangeRateUpdater.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.Providers
{
    internal class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        private const string ExchangeRatesCacheKey = "CzechNationalBankExchangeRatesCacheKey";

        private readonly ICzechNationalBankApiClient _apiClient;
        private readonly ICache _cache;
        private readonly IOptions<CzechNationalBankApiSettings> _options;
        private readonly ILogger<CzechNationalBankExchangeRateProvider> _logger;

        public CzechNationalBankExchangeRateProvider(ICzechNationalBankApiClient apiClient, ICache cache, IOptions<CzechNationalBankApiSettings> options, ILogger<CzechNationalBankExchangeRateProvider> logger)
        {
            _apiClient = apiClient;
            _cache = cache;
            _options = options;
            _logger = logger;
        }

        public CurrencyCode[] SupportedCurrencies => new[] { CurrencyCode.CZK };

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CurrencyCode targetCurrency)
        {
            _logger.LogInformation("CzechNationalBankExchangeRateProvider.GetExchangeRatesAsync: Getting exchange rates for currencies {currencies}", string.Join(", ", currencies));
            
            var exchangeRatesResponse = await GetExchangeRatesResponseAsync();
            var currencyCodes = currencies.Select(c => c.Code.ToString());

            return exchangeRatesResponse
                    .Rates
                    .Where(r => currencyCodes.Contains(r.CurrencyCode))
                    .Select(r => MapExchangeRate(r, targetCurrency));

        }

        private async Task<CzechNationalBankExchangeRatesResponse> GetExchangeRatesResponseAsync()
        {
            try
            {
                var todayExchangeRatesResponse = _cache.Get<CzechNationalBankExchangeRatesResponse>(ExchangeRatesCacheKey);
                if (todayExchangeRatesResponse == null)
                {
                    todayExchangeRatesResponse = await _apiClient.GetExchangeRatesAsync(DateTime.UtcNow);
                    if (!todayExchangeRatesResponse.Rates.Any())
                    {
                        return new CzechNationalBankExchangeRatesResponse();
                    }

                    var validFor = DateTime.Parse(todayExchangeRatesResponse.Rates.First().ValidFor);
                    _cache.Set(ExchangeRatesCacheKey, todayExchangeRatesResponse, validFor.AddDays(1));
                }
                else
                {
                    _logger.LogInformation("CzechNationalBankExchangeRateProvider.GetExchangeRatesAsync: Got exchange rates from cache");
                }

                return todayExchangeRatesResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get exchange rates from CzechNationalBank API");
                throw;
            }
        }

        private static ExchangeRate MapExchangeRate(CzechNationalBankExchangeRate bankExchangeRate, CurrencyCode targetCurrency)
        {
            return new ExchangeRate(
                        new Currency(bankExchangeRate.CurrencyCode),
                        new Currency(targetCurrency),
                        Math.Round(bankExchangeRate.Rate / bankExchangeRate.Amount, 2));
        }
    }
}
