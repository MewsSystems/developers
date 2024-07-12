using CzechNationalBankApi;
using ExchangeRateUpdater.Application;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

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
            return Enumerable.Empty<ExchangeRate>();
        }
    }
}
