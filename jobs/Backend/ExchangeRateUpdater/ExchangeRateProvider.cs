using Core.Models;
using Core.Models.CzechNationalBank;
using ExchangeRateUpdater.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private ILogger _logger;
        private IConfiguration _configuration;
        private IClient<CzechNationalBankExchangeRateItem> _client;

        private const string CZK_CODE = "CZK";
        private Currency targetCurrency = new Currency(CZK_CODE);

        public ExchangeRateProvider(ILogger<ExchangeRateProvider> logger, IConfiguration configuration, IClient<CzechNationalBankExchangeRateItem> client)
        {
            _logger = logger;
            _configuration = configuration;
            _client = client;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = Enumerable.Empty<ExchangeRate>();
            var calculatedRates = new List<ExchangeRate>();

            try
            {
                var rateData = await _client.GetExchangeRates();
                _logger.LogDebug($"Rate data \r\n");
                foreach (var data in rateData.ToList())
                {
                    _logger.LogDebug($"Currency: {data.Currency}");
                    _logger.LogDebug($"Code: {data.Code}");
                    _logger.LogDebug($"Rate: {data.Rate}");
                    _logger.LogDebug($"Amount: {data.Amount}");
                }

                foreach (BaseExchangeRateItem cnbData in rateData)
                {
                    calculatedRates.Add(new ExchangeRate(
                        new Currency(cnbData.Code),
                        targetCurrency,
                        cnbData.Rate / cnbData.Amount
                        ));
                }

                rates = calculatedRates.Where(rates => currencies.Any(currency => currency.Code.Equals(rates.SourceCurrency.Code)));
            } 
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve and parse exchange rates: {Environment.NewLine}");
                _logger.LogError(ex, ex.Message);
            }

            return rates;
        }
    }
}
