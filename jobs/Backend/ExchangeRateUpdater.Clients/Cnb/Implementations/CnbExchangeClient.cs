using ExchangeRateUpdater.BusinessLogic.Models.Cnb.Constants;
using ExchangeRateUpdater.Clients.Cnb.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Clients.Cnb.Implementations
{
    public class CnbExchangeClient : ICnbExchangeClient
    {
        private readonly IConfigurationSection _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public CnbExchangeClient(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = config.GetSection(CnbConstants.SettingsSectionKey);
        }

        public async Task<string> GetExchangeRateTxtAsync(string currencyCode)
        {
            ArgumentNullException.ThrowIfNull(currencyCode);

            var cnbUrl = _configuration.GetRequiredSection(CnbConstants.SettingsDailyExChangeRateUrlKey).Value;

            var client = _httpClientFactory.CreateClient();
            var resultCnb = await client.GetStringAsync(cnbUrl);

            return resultCnb;
        }

        public async Task<string> GetFxExchangeRateTxtAsync(string currencyCode)
        {
            ArgumentNullException.ThrowIfNull(currencyCode);

            var cnbFxUrl = _configuration.GetRequiredSection(CnbConstants.SettingsMonthlyFxExChangeRateUrlKey).Value;
            var year = DateTime.UtcNow.Month == 1 ? DateTime.UtcNow.Year - 1 : DateTime.UtcNow.Year;

            var client = _httpClientFactory.CreateClient();
            var resultCnb = await client.GetStringAsync(cnbFxUrl + $"?currency={currencyCode}&country=&from=01+Jan+{year}&to=31+Dec+{year}");

            return resultCnb;
        }
    }
}