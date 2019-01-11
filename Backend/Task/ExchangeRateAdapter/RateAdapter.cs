using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ExchangeRateService;
using ExchangeRateUpdater;

namespace ExchangeRateAdapter
{
    public class RateAdapter
    {
        public RateAdapter()
        {
            // Create new instance of the concrete CNBExchangeRateService. 
            // If new data sources are required later any class which implements the
            // IExchangeService interface can be used here. 
            ExchangeRateService = new CNBExchangeRateService();
        }

        public IExchangeRateService ExchangeRateService { get; set; }

        /// <summary>
        /// Retrieve a list of exchange rate data using an ExchangeRateService
        /// </summary>
        /// <param name="currencyCodes">List of desired currency codes</param>
        /// <param name="sourceCurrencyCode">The currency code specifying the source currency of the data source</param>
        /// <param name="getCalculatedRates">Optional flag to determine if the operation should calculate inferred rates</param>
        /// <returns>A list of exchange rates giving the source currency code, target currency code and the conversion rate</returns>
        public IEnumerable<ExchangeRate> GetExchangeRateData(IEnumerable<string> currencyCodes, string sourceCurrencyCode, bool getCalculatedRates = false)
        {
            var result = new List<ExchangeRate>();
            try
            {
                List<CurrencyData> currencyDataList = ExchangeRateService.GetExchangeRateData(currencyCodes).ToList();

                if (currencyDataList.Count == 0) throw new Exception("No exchange rate data was returned");

                // If the optional getCalculatedRates flag is set, generate calculated exchange rates
                // between currencies
                if (getCalculatedRates)
                {
                    foreach (var currency in currencyDataList)
                    {
                        // Iterate through every requested currency except the current one
                        // to calculate the conversions
                        var conversionCurrencyList = currencyDataList.Where(c => c.CurrencyCode != currency.CurrencyCode);
                        foreach (var conversionCurrency in conversionCurrencyList)
                        {
                            result.Add(calculateExchangeRate(currency, conversionCurrency));
                        }
                    }
                }
                else
                {
                    // No calculations to be performed so simply calculate the exchange rate for every
                    // requested currency against the course currency.Filtering out the source currency
                    // here to avoid displaying the 1:1 conversion.
                    foreach (var currency in currencyDataList.Where(cd => cd.CurrencyCode != sourceCurrencyCode))
                    {
                        result.Add(calculateExchangeRate(currency, new CurrencyData(sourceCurrencyCode, 1m, 1)));
                    }
                }

                return result.OrderBy(c => c.SourceCurrency.Code);
            }
            catch(ApplicationException ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// Calculates the exchange rate between two currencies  based on a known exchange rate
        /// shared between both currencies
        /// </summary>
        /// <param name="sourceCurrency">The source currency data</param>
        /// <param name="targetCurrency">The target currency data</param>
        /// <returns></returns>
        private ExchangeRate calculateExchangeRate(CurrencyData sourceCurrencyData, CurrencyData targetCurrencyData)
        {
            var value = (sourceCurrencyData.Value / sourceCurrencyData.Amount) / (targetCurrencyData.Value / targetCurrencyData.Amount);
            var exchangeRate = new ExchangeRate(
                new Currency(sourceCurrencyData.CurrencyCode),
                new Currency(targetCurrencyData.CurrencyCode),
                value);
            return exchangeRate;
        }
    }
}
