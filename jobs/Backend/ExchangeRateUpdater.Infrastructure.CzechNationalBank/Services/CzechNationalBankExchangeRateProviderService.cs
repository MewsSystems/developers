using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Models;
using Serilog;
using System.Text.Json;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.Services
{
    public class CzechNationalBankExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        private readonly Currency _targetCurrency = new("CZK");

        public CzechNationalBankExchangeRateProviderService(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<NonNullResponse<Dictionary<string, ExchangeRate>>> GetExchangeRates()
        {
            var rates = new Dictionary<string, ExchangeRate>();
            try
            {
                var cnbClient = _httpClientFactory.CreateClient("CzechNationalBankApi");
                var todayDate = DateTime.Now.ToString("yyyy-MM-dd");

                var response = await cnbClient.GetAsync($"exrates/daily?date={todayDate}&lang=EN");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.Error("The api has responded with {code}: {@response}",response.StatusCode,response);
                    return NonNullResponse<Dictionary<string, ExchangeRate>>.Fail(rates,"Api response was not successful");
                }
                var exchangeRates = JsonSerializer.Deserialize<DailyRatesResponse>(await response.Content.ReadAsStringAsync());
                if (exchangeRates != null)
                {
                    rates = exchangeRates.Rates.ToDictionary(rate => rate.CurrencyCode, rate => new ExchangeRate(new Currency(rate.CurrencyCode),_targetCurrency, rate.Rate));
                }
                return NonNullResponse<Dictionary<string, ExchangeRate>>.Success(rates);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Error while retrieving exchanges");
                return NonNullResponse<Dictionary<string, ExchangeRate>>.Fail(rates, exception.Message);
            }
        }
    }
}
