using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IExchangeRateService exchangeRateService = new CnbExchangeRateService();
            IExchangeRateProvider exchangeRateProvider = new ExchangeRateProvider(exchangeRateService);

            var currencies = new List<Currency>
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

            var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

            foreach (var rate in exchangeRates)
            {
                Console.WriteLine(rate);
            }
        }
    }
}
