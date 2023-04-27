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
            var exchangeRatesDataSource = _configuration.GetSection("ExchangeRateDateSource").Value;

            if (string.IsNullOrEmpty(exchangeRatesDataSource))
            {
                throw new ArgumentException("Exchange rate data source is not configured");
            }

            if (!File.Exists(exchangeRatesDataSource))
            {
                throw new FileNotFoundException("Exchange rate data source file does not exist", exchangeRatesDataSource);
            }

            string exchangeRatesDataSourceContent = "";

            try
            {
                exchangeRatesDataSourceContent = await _httpClient.GetStringAsync(exchangeRatesDataSource);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while fetching exchange rates data source content", ex);
            }

            if (string.IsNullOrEmpty(exchangeRatesDataSourceContent))
            {
                throw new Exception("Exchange rate data source content is null or empty");
            }

            return _exchangeRateFormatter
                .FormatExchangeRates(exchangeRatesDataSourceContent)
                .Where(exchangeRate => currencies.Any(currency => currency.Code == exchangeRate.TargetCurrency.Code));
        }

    }
}
