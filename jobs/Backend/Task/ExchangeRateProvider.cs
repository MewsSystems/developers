using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRateProvider
    {
        public CNBExchangeRateProvider(HttpClient client)
        {
            _client = client;
        }

        // empty list for CNB website addresses of the rate sources (all of the sources are formated in the same way so it can be parsed together)
        private List<string> _urlSources = new List<string>()
        {
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt",
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt"
        };

        private readonly HttpClient _client;

        // everything from the source is compared against CZK, so it only has to be instantiated once and then simply referenced in all the ExchangeRates instances
        private readonly Currency _sourceCurrency = new Currency("CZK");





        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            
            var outputListOfExchangeRates = new List<ExchangeRate>();
            var allAccessibleRates = new List<(string, int, decimal)>();
            var remainingCurrenciesToLookFor = currencies.ToList();

            // gets all possible rates from urls in an "allAccessibleRates" list
            foreach (var url in _urlSources)
            {
                string rawData = GetRequestRawExchangeData(url).Result;
                ParseRawDataToListOfTuples(rawData).ForEach(item => allAccessibleRates.Add(item));
            }

            // if there are no remaining currencies as all requested have been already found - break
            // if requested data is found in parsed source then add it to the output list that will then be returned by this method 
            foreach (var rate in allAccessibleRates)
            {
                if (remainingCurrenciesToLookFor.Count > 0)
                {
                    foreach (var currency in remainingCurrenciesToLookFor)
                    {
                        if (rate.Item1 == currency.Code)
                        {
                            remainingCurrenciesToLookFor.Remove(currency);

                            // value is divided by the amount so that if the source claims eg. 100 rubles for 46czk, then price for a single ruble (0.46) is output
                            decimal value = rate.Item3 / rate.Item2;

                            // ( code , amount , value )
                            var rateTuple = new ExchangeRate(_sourceCurrency, new Currency(rate.Item1), value);
                            outputListOfExchangeRates.Add(rateTuple);
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            return outputListOfExchangeRates;
        }



        public List<(string, int, decimal)> ParseRawDataToListOfTuples(string rawData)
        {
            // each item of the list is line of the web text file
            List<string> rawLines = rawData.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();

            // empty list for extracted data that will be saved in the following order:  
            List<(string, int, decimal)> parsedLines = new List<(string, int, decimal)>();


            // get column numbers of the specific data
            var secondLine = rawLines[1].Split('|').ToList();
            int amountIndex = secondLine.IndexOf("Amount");
            int codeIndex = secondLine.IndexOf("Code");
            int valueIndex = secondLine.IndexOf("Rate");

            // remove second line that contains names of the columns
            rawLines.RemoveAt(1);
            // remove first line that contains date and #number of the release for the year
            rawLines.RemoveAt(0);

            foreach (var line in rawLines)
            {
                string[] splitLine = line.Split('|');

                // tuple is formatted as follows: ( curr_symbol , amount , rate )
                parsedLines.Add((splitLine[codeIndex], Convert.ToInt32(splitLine[amountIndex]), Convert.ToDecimal(splitLine[valueIndex])));
            }
            return parsedLines;
        }





        public async Task<string> GetRequestRawExchangeData(string url)
        {
            HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
