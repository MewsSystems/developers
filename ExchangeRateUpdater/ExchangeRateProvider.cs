using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICurrencyConverterApi _currencyConverterApi;

        private readonly Currency _baseCurrency;

        private readonly IExchangeRateParser _exchangeRateParser;

        public ExchangeRateProvider(ICurrencyConverterApi currencyConverterApi, 
            Currency baseCurrency, IExchangeRateParser exchangeRateParser)
        {
            _currencyConverterApi = currencyConverterApi;
            _baseCurrency = baseCurrency;
            _exchangeRateParser = exchangeRateParser;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            foreach (var currency in currencies)
            {
                JObject exchangeRateResponse;

                try
                {
                    exchangeRateResponse = _currencyConverterApi.GetExchangeRate(new GetExchangeRateRequest
                    {
                        Query = $"{_baseCurrency.Code}_{currency.Code}"
                    }).Result;
                }
                catch (AggregateException e)
                {
                    throw new Exception(string.Join(", ", e.InnerExceptions.Select(x => x.Message)));
                }

                var exchangeRate =
                    _exchangeRateParser.ParseExchangeRateResponse(exchangeRateResponse,
                        $"{_baseCurrency.Code}_{currency.Code}");
                
                if (exchangeRate == null) continue;

                // We ccould use this converter in another cases against one in Program.cs and it will be good to have
                // opportunity to operate results till potential error occurs 
                yield return new ExchangeRate(_baseCurrency, currency, exchangeRate.Value);
            }
        }
    }
}