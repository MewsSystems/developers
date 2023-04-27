using ExchangeRateUpdaterAPI.Services.ExchangeRateFormatterService;
using ExchangeRateUpdaterAPI.Services.ExchangeRateProviderService;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IExchangeRateFormatter _exchangeRateFormatter;

        public ExchangeRateProvider(IConfiguration configuration, HttpClient httpClient, IExchangeRateFormatter exchangeRateFormatter)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _exchangeRateFormatter = exchangeRateFormatter;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // TODO check if file is null pr if it exists
            var exchangeRatesDataSource = _configuration.GetSection("ExchangeRateDateSource").Value;

            // TODO Error handling
            var exchangeRatesDataSourceContent = await _httpClient.GetStringAsync(exchangeRatesDataSource);

            return _exchangeRateFormatter
                .FormatExchangeRates(exchangeRatesDataSourceContent)
                .Where(exchangeRate => currencies.Any(currency => currency.Code == exchangeRate.TargetCurrency.Code));
        }
    }
}
