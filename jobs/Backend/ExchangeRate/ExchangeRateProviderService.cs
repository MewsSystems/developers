using CzechNationalBankClient;
using CzechNationalBankClient.Model;
using ExchangeRateProvider.Objects;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider
{
    public class ExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly ICurrencyExchangeRateClient _currencyExchangeRateClient;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ILogger<IExchangeRateProviderService> _logger;
        private const int IsoCodeLength = 3;
        private const string Language = "EN";
        private const string CZK = "CZK";

        public ExchangeRateProviderService(ICurrencyExchangeRateClient currencyExchangeRateClient,
            IMemoryCacheService memoryCacheService, ILogger<IExchangeRateProviderService> logger)
        {
            _currencyExchangeRateClient = currencyExchangeRateClient;
            _memoryCacheService = memoryCacheService;
            _logger = logger;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<List<ExchangeRate>> RetrieveExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime date)
        {
            var cnbRates = await GetCnbCurrencyRatesAsync(date);
            var cnbOtherRates = await GetCnbOtherCurrencyRatesAsync(date);

            var exchangeRates = new List<ExchangeRate>();
            foreach (var currency in currencies)
            {
                if (currency?.Code == null || currency.Code.Length != IsoCodeLength)
                {
                    _logger.LogInformation("Currency code not valid: {currency}", currency?.Code);
                    continue;
                }
                var exchangeRate = cnbRates.FirstOrDefault(rate => rate.CurrencyCode.Equals(currency.Code, StringComparison.InvariantCultureIgnoreCase))
                    ?? cnbOtherRates.FirstOrDefault(rate => rate.CurrencyCode.Equals(currency.Code, StringComparison.InvariantCultureIgnoreCase));
                if (exchangeRate == null)
                {
                    _logger.LogInformation("Exchange rate not found for currency: {currency}", currency.Code);
                    continue;
                }
                exchangeRates.Add(BuildExchangeRate(exchangeRate));
            }
            return exchangeRates;
        }

        private async Task<IEnumerable<CnbExchangeRate>> GetCnbCurrencyRatesAsync(DateTime date)
        {
            var formatedDate = date.ToString("yyyy-MM-dd");
            var rates = _memoryCacheService.GetCachedRatesValue(formatedDate);
            if (rates == null)
            {
                _logger.LogInformation("cnb currency rates not found in cache, getting them from source...");
                try
                {
                    rates = await _currencyExchangeRateClient.GetCurrencyExchangeRatesAsync(formatedDate, Language);
                    _logger.LogInformation("cnb currency rates found: {rates}", rates.Count());
                    _memoryCacheService.SetCachedRates(formatedDate, rates);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Something bad happened when trying to get currency exchange rates: {exception}", ex.Message);
                    throw;
                }
            }
            return rates;
        }

        private async Task<IEnumerable<CnbExchangeRate>> GetCnbOtherCurrencyRatesAsync(DateTime date)
        {
            var formatedDate = date.ToString("yyyy-MM");
            var rates = _memoryCacheService.GetCachedRatesValue(formatedDate);
            if (rates == null)
            {
                _logger.LogInformation("cnb other currency rates not found in cache, getting them from source...");
                try
                {
                    rates = await _currencyExchangeRateClient.GetOtherCurrencyExchangeRatesAsync(formatedDate, Language);
                    _logger.LogInformation("cnb currency rates found: {rates}", rates.Count());
                    _memoryCacheService.SetCachedRates(formatedDate, rates);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Something bad happened when trying to get other currency exchange rates: {exception}", ex.Message);
                    throw;
                }
            }
            return rates;
        }

        private static ExchangeRate BuildExchangeRate(CnbExchangeRate cnbExchangeRate)
        {
            return new(new Currency(cnbExchangeRate.CurrencyCode.ToUpper()), new Currency(CZK), cnbExchangeRate.Rate);
        }
    }
}