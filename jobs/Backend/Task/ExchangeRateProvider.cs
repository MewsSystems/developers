using ExchangeRateUpdater.Log;
using ExchangeRateUpdater.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {

        private ILogger _log;


        public ExchangeRateProvider(ILogger log) => _log = log;

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            _log.LogInfo("Retrieving data from web.");
            List<CnbExchangeRateData> data = CnbExchangeRate.GetCnbExchangeRates();

            _log.LogInfo("Creating the exchange rates.");
            List<ExchangeRate> exchangeRates = CrateExchangeRates(data, currencies);
            _log.LogInfo("The exchange rates was successfully created.");
            return exchangeRates;
        }

        private List<ExchangeRate> CrateExchangeRates(List<CnbExchangeRateData> ExchangeRateData, IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new();

            foreach(CnbExchangeRateData c in ExchangeRateData)
            {
                if(currencies.Any( a => a.Code == c.Code))
                {
                    Currency source = new (c.Code);
                    Currency target = new ("CZK");

                    int amount = Convert.ToInt32(c.Amount);
                    decimal.TryParse(c.Rate, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal rate);
                    exchangeRates.Add(new ExchangeRate(source, target, amount, rate));

                    if (c.Rate == "0")
                    {
                        _log.LogInfo($"The rate of {c.Code}/CZK is 0. Cannot create CZK/{c.Code}");
                        continue;
                    }
                    if (amount > 1)
                    {
                        source = new ("CZK");
                        target = new (c.Code);
                        rate = amount / rate; ;
                        rate = Math.Truncate(rate * 1000) / 1000;
                        exchangeRates.Add(new ExchangeRate(source, target, 1, rate));
                    }
                }
            }

            return exchangeRates;
        }

        
    }
}
