using System.Collections.Generic;
using System.Linq;
//Add new libraries for the implementation
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;

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
        
        private static readonly HttpClient _httpClient = new HttpClient(); //client for making the request
        private const string CNB_URL = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";//url for retrieving the info
        
        
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try 
            {
                var  cnbData  = DownloadCnbData();
                var  exchangeRates  = ParseCnbData(cnbData);
                var  requestedCodes  = currencies.Select(c  => c.Code).ToHashSet();
                var  results  = new  List<ExchangeRate>();
                
                foreach  (var  rate  in  exchangeRates)
                {
                    if  (requestedCodes.Contains(rate.CurrencyCode))
                    {
                        var  czk  = new  Currency("CZK");
                        var  target  = new  Currency(rate.CurrencyCode);
                        var  exchRate  = new  ExchangeRate(czk,  target,  rate.Rate);
                        results.Add(exchRate);
                    }
                }
                
                return  results;
            }
            catch  (Exception  ex)
            {
                Console.WriteLine($"Error:  {ex.Message}");
                return  Enumerable.Empty<ExchangeRate>();
            }
        }

private  List<(string  CurrencyCode,  decimal  Rate)>  ParseCnbData(string  cnbData)
        {
            var  rates  = new  List<(string  CurrencyCode,  decimal  Rate)>();
            var  lines  = cnbData.Split('\n',  StringSplitOptions.RemoveEmptyEntries);
            
            for  (int  i  = 2;  i  < lines.Length;  i++)
            {
                var  parts  = lines[i].Split('|');
                if  (parts.Length  >= 5)
                {
                    try
                    {
                        var  currencyCode  = parts[3].Trim();
                        var  amount  = decimal.Parse(parts[2].Trim());
                        var  rate  = decimal.Parse(parts[4].Trim(),  CultureInfo.InvariantCulture);
                        
                        var  normalizedRate  = rate  / amount;
                        rates.Add((currencyCode,  normalizedRate));
                        
                        Console.WriteLine($"Parsed:  {currencyCode}  =  {normalizedRate}");
                    }
                    catch  (Exception  ex)
                    {
                        Console.WriteLine($"Error  parsing  line:  {lines[i]}  -  {ex.Message}");
                    }
                }
            }
            
            return  rates;
        }

        private  string  DownloadCnbData()
        {
            var  task  =  _httpClient.GetStringAsync(CNB_URL);
            return  task.Result;
        }
    }
}
