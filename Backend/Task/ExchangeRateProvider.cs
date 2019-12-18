using CNB;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        public IServiceProvider Services { get; private set; }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            List<Currency> missingRates = new List<Currency>();

            //dependency injection
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddScoped<CnbClient>();
            Services = services.BuildServiceProvider();

            var client = Services.GetRequiredService<CnbClient>();

            var BasicRates = client.ExchangeRateAll(null, "EN").Result;

            foreach (var currency in currencies)
            {
                var currencyRead = BasicRates.FirstOrDefault(p => p.Code == currency.Code);
                if (currencyRead != null)
                {
                    var newRate = new ExchangeRate(new Currency(currencyRead.Code), new Currency("CZK"), currencyRead.Rate / currencyRead.Amount);
                    exchangeRates.Add(newRate);
                }
                else
                {
                    if (currency.Code != "CZK") // Czech national bank will not have rate for its own currency, it is not a reason to load extended list 
                        missingRates.Add(currency);
                }

            }

            //extended list is loaded only when needed
            if (missingRates.Count > 0)
            {
                var extendedrates = client.ExchangeRateOther(null, "EN").Result;

                foreach (var currency in missingRates)
                {
                    var currencyRead = extendedrates.FirstOrDefault(p => p.Code == currency.Code);
                    if (currencyRead != null)
                    {
                        var newRate = new ExchangeRate(new Currency(currencyRead.Code), new Currency("CZK"), currencyRead.Rate / currencyRead.Amount);
                        exchangeRates.Add(newRate);
                    }
                }
            }

            return exchangeRates;
        }
    }
}
