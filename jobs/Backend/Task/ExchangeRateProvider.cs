using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var downloadedExchangeRateData = GetTodaysExchangeRatesFromWeb();
            var parsedExchangeRates = ProcessDownloadedFile(downloadedExchangeRateData);
         
            // return only those exchange rates, which are included in the input parameter
            return parsedExchangeRates.Where(per => currencies.Select(c => c.Code).Contains(per.TargetCurrency.Code));
        }


        /// <summary>
        /// Gets data from CNB web for today
        /// </summary>
        /// <returns>String of downloaded data</returns>
        private string GetTodaysExchangeRatesFromWeb()
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.BaseAddress = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

                    return client.DownloadString(client.BaseAddress + "?date=" + DateTime.Today.ToString("dd.MM.yyyy"));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Parses downloaded exchange rate string 
        /// </summary>
        /// <param name="exchangeRateData">Downloaded string</param>
        /// <returns>Enumerable of ExchangeRate</returns>
        private IEnumerable<ExchangeRate> ProcessDownloadedFile(string exchangeRateData)
        {
            var exchangeRateList = new List<ExchangeRate>();
            
            // split into line on new line (\n) 
            var lines = exchangeRateData.Split("\n");

            // if there are less than 3 lines (date, headers, 1 row) return empty
            if (lines.Length <= 2)
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            // source set to CZE due to data source being Czech National Bank
            var sourceCurrency = new Currency("CZE");

            // process each data line - after first 2 (date, header)
            foreach (var line in lines.Skip(2))
            {
                // checks if we can split the line
                if (line.Contains('|'))
                {
                    // gets values 
                    var columns = line.Split('|');

                    // if we have correct column count (5), parse the string and create exchange rate
                    if(columns.Length == 5)
                    {
                        var targetCurrency = new Currency(columns[3]);
                        int.TryParse(columns[2], out int amount);
                        var rate = decimal.Parse(columns[4], CultureInfo.InvariantCulture);
              
                        exchangeRateList.Add(new ExchangeRate(sourceCurrency, targetCurrency, rate / amount));
                    }
                }
            }

            return exchangeRateList;
        }
    }
}
