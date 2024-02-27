using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpApiService;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        public ExchangeRateProvider() { }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<List<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, ILogger logger)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            try
            {
                HttpService httpService = new HttpService(logger);

                string currentUtcDateString = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");

                logger.LogInformation("Attempting to retrieve exchange rate data from API");

                ExchangeRateApiResponse exchangeRateApiResponse = await httpService.GetWithJsonMapping<ExchangeRateApiResponse>
                ($"https://api.cnb.cz/cnbapi/exrates/daily?date={currentUtcDateString}&lang=EN");

                if (exchangeRateApiResponse != null)
                {
                    List<ExchangeRateApiData> filteredExchangeRateData =
                        exchangeRateApiResponse.Rates.Where(x => currencies.Any(y => y.Code == x.CurrencyCode)).ToList();

                    logger.LogInformation($"{filteredExchangeRateData.Count} rates remaining from {exchangeRateApiResponse.Rates.Count} total after filtering");

                    foreach (ExchangeRateApiData exchangeRate in filteredExchangeRateData)
                    {
                        exchangeRates.Add(new ExchangeRate(currencies.FirstOrDefault(x => x.Code == exchangeRate.CurrencyCode), currencies.FirstOrDefault(x => x.Code == "CZK"), exchangeRate.Rate));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"ExchangeRateProvider encountered an unhandled exception: {ex.Message}");
                throw;
            }

            return exchangeRates;
        }
    }
}
