using ExchangeRateUpdater.Core.Exceptions;
using ExchangeRateUpdater.Core.Models.CzechNationalBank;
using ExchangeRateUpdater.Core.Providers;
using ExchangeRateUpdater.CzechNationalBank.Sources;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICzechNationalBankSource _czechNationalBankApi;
        private readonly ILogger<CzechNationalBankExchangeRateProvider> _logger;

        public CzechNationalBankExchangeRateProvider(
            ICzechNationalBankSource czechNationalBankApi,
            ILogger<CzechNationalBankExchangeRateProvider> logger)
        {
            _czechNationalBankApi = czechNationalBankApi;
            _logger = logger;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<string> currencies)
        {
            var rates = new List<CzechNationalBankExchangeRate>();

            var exchangeRatesDailyDto = await _czechNationalBankApi.GetExchangeRatesAsync();
            if (exchangeRatesDailyDto == null || !exchangeRatesDailyDto.Rates.Any())
            {
                _logger.LogDebug("Did not retrieve exchange rates");
                return rates;
            }

            var currencyCodes = currencies.Select(x => x.ToUpper()).ToHashSet();
            var availableCurrencyRates = exchangeRatesDailyDto.Rates.Where(x => currencyCodes.Contains(x.CurrencyCode.ToUpper()));
            foreach (var currencyRate in availableCurrencyRates)
            {
                try
                {
                    rates.Add(new CzechNationalBankExchangeRate(currencyRate.Amount, currencyRate.Rate, currencyRate.CurrencyCode));
                }
                catch (ExchangeRateException e)
                {
                    _logger.LogError(e, $"Failed to calculate exchange rate");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Failed to calculate exchange rate of currency code: {currencyRate.CurrencyCode}");
                }
            }

            return rates;
        }
    }
}
