using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.HttpUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const int MaxRetryCount = 5;
        private readonly ICnbClient _cnbClient;

        public ExchangeRateProvider(ICnbClient cnbClient)
        {
            _cnbClient = cnbClient;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if(currencies == null)
            {
                throw new ExchangeRatesException("The list of currencies is not provided", false);
            }

            if(!currencies.Any()) 
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            var retryCount = 0;
            while (retryCount < MaxRetryCount)
            {
                try
                {
                    return await _cnbClient.GetCurrentExchangeRatesAsync(currencies);
                }
                catch (ExchangeRatesException ex)
                {
                    if(ex.Retriable == false) throw ex;

                    retryCount++;
                    await Task.Delay(retryCount * 1000);
                }
            }

            throw new ExchangeRatesException($"The source failed to return exchange rates after {MaxRetryCount} retries", false);
        }
    }
}
