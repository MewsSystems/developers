using CzechNationalBankApi;
using ExchangeRateUpdater.Application;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly ICzechBankApiService _czechBankApiService;

        public ExchangeRateProvider(ILogger<ExchangeRateProvider> logger, ICzechBankApiService czechBankApiService)
        {
            _logger = logger;
            _czechBankApiService = czechBankApiService;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (!currencies.Any())
            {
                _logger.LogInformation("No currencies passed, returning empty list of exchange rates");

                return Enumerable.Empty<ExchangeRate>();
            }

            var exchangeRatesFromCzechBank = await _czechBankApiService.GetExchangeRatesAsync();

            if (!exchangeRatesFromCzechBank.Any())
            {
                _logger.LogError("No exchange rates were returned from the Czech bank api");

                return Enumerable.Empty<ExchangeRate>();
            }

            var exchangeRates = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                var exchangeData = exchangeRatesFromCzechBank.FirstOrDefault(x => x.Code.Equals(currency.Code, StringComparison.InvariantCultureIgnoreCase));

                if(exchangeData != null)
                {
                    exchangeRates.Add(new ExchangeRate(currency, new Currency("CZK"), exchangeData.Rate));
                }
                else
                {
                    _logger.LogInformation($"Couldn't find currency {currency.Code} in exchange rate data, skipping");

                    continue;
                }
            }

            return exchangeRates;
        }
    }
}
