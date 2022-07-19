using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // everything from the source is compared against CZK, so it only has to be instantiated once and then simply referenced in all the ExchangeRates instances
            var sourceCurrency = new Currency("CZK");

            // empty list for CNB website addresses of the rate sources (all of them are formated in the same way so it can be parsed together)
            List<string> urlSources = new List<string>();


            // (hardcoded urls - i am not aware of the level of generalization to aim for in this case and creating specific class for other national banks (sources) from other countries seems like an overkill so i just left it like this)
            // common currencies - updated every working day at 14:30 CEST
            urlSources.Add("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
            
            // less common currencies - updated monthly
            urlSources.Add("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt");

            var outputListOfExchangeRates = new List<ExchangeRate>();
            var allAccessibleRates = new List<(string, int, decimal)>();
            var remainingCurrenciesToLookFor = currencies.ToList();

            // gets all possible rates from urls in an "allAccessibleRates" list
            foreach (var url in urlSources)
            {
                string rawData = CollectRawExchangeData(url);
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
                            
                            // value is divided by the amount so that if the source claims eg. 100 rubles for 46czk, then price for single ruble (0.46) is output
                            decimal value = rate.Item3 / rate.Item2;

                            // ( code , amount , value )
                            var rateTuple = new ExchangeRate(sourceCurrency, new Currency(rate.Item1), value);
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


            // get column numbers of the specific data ( I made this part dynamic simply if for some reason the order should be changed by the bank down the line even tho its probably unlikely to ever happen)
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
                parsedLines.Add( (splitLine[codeIndex] , Convert.ToInt32(splitLine[amountIndex]) , Convert.ToDecimal(splitLine[valueIndex])) );
            }

            return parsedLines;
        }


        public string CollectRawExchangeData(string url)
        {
            // (could perhaps be replaced with HttpClient or RestSharp, but in this simple case WebClient seems to be OK, besides it should have faster response times) 
            WebClient wc = new WebClient();
            return wc.DownloadString(url);
        }
    }
}
