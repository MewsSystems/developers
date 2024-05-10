using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.Core.Providers;
using ExchangeRateUpdater.CzechNationalBank.Api;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICzechNationalBankApi _czechNationalBankApi;
        private readonly CzechNationalBankConfiguration _configuration;
        private readonly ILogger<CzechNationalBankExchangeRateProvider> _logger;

        public CzechNationalBankExchangeRateProvider(
            ICzechNationalBankApi czechNationalBankApi,
            CzechNationalBankConfiguration configuration,
            ILogger<CzechNationalBankExchangeRateProvider> logger)
        {
            _czechNationalBankApi = czechNationalBankApi;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
        {
            var rates = new List<ExchangeRate>();

            var exchangeRatesDailyDto = await _czechNationalBankApi.GetExchangeRatesAsync();
            if (exchangeRatesDailyDto == null || !exchangeRatesDailyDto.Rates.Any())
            {
                _logger.LogDebug("Did not retrieve exchange rates");
                return rates;
            }

            var currencyCodes = GetCurrencies().Select(x => x.Code.ToUpper()).ToHashSet();
            var availableCurrencyRates = exchangeRatesDailyDto.Rates.Where(x => currencyCodes.Contains(x.CurrencyCode.ToUpper()));
            foreach (var currencyRate in availableCurrencyRates)
            {
                var value = Math.Round(currencyRate.Amount / currencyRate.Rate, _configuration.DecimalPlaces);
                var rate = new ExchangeRate(new Currency(currencyRate.CurrencyCode), new Currency(_configuration.DefaultCurrencyCode), value);
                rates.Add(rate);
            }

            return rates;
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            return new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"),
                new Currency("JPY"),
                new Currency("KES"),
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")
            };
        }
    }
}
