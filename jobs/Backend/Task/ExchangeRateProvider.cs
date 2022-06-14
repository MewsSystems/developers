using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            //Can create custom exchange rates. CZK/CZK should always be 1.
            string[] customExchangeRates = new[]
            {
                "Czech Republic|crown|1|CZK|1",
            };

            //URLs of sites that we then take exchange rates from.
            List<String> listOfWebsites = new List<string>
            {
                "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt",
                "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-ostatnich-men/kurzy-ostatnich-men/kurzy.txt",
            };

            List<ExchangeRate> allExchangeRates = GetListOfExchangeRates(listOfWebsites, customExchangeRates);

            return allExchangeRates.Where(er => GetCodesOfCurrencies(currencies).Contains(er.SourceCurrency.Code));
        }

        //Gets string codes of currencies for better comparison.
        public List<String> GetCodesOfCurrencies(IEnumerable<Currency> currencies)
        {
            List<String> codesOfCurrencies = new List<string>();

            foreach (var currency in currencies)
            {
                codesOfCurrencies.Add(currency.Code);
            }

            return codesOfCurrencies;
        }

        //Gets exchange rates from all sites and returns only those that are valid.
        public List<ExchangeRate> GetListOfExchangeRates(List<String> listOfWebsites, string[] customExchangeRates)
        {
            WebClient myWebClient = new WebClient();

            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            
            foreach (string website in listOfWebsites)
            {
                Stream myStream = myWebClient.OpenRead(website);
               
                using (StreamReader sr = new StreamReader(myStream))
                {
                    while (sr.Peek() >= 0)
                    {
                        ExchangeRate newExchangeRate = ProcessLine(sr.ReadLine());

                        if (newExchangeRate != null)
                        {
                            exchangeRates.Add(newExchangeRate);
                        }
                    }
                }

                myStream.Close();               
            }
            
            foreach (string rate in customExchangeRates)
            {
                ExchangeRate newExchangeRate = ProcessLine(rate);

                if (newExchangeRate != null)
                {
                    exchangeRates.Add(newExchangeRate);
                }
            }

            return exchangeRates;
        }

        //Checks if line from the site is in a valid format and contains the exchange rate of the currencies we wanted.
        public ExchangeRate ProcessLine(string line)
        {
            string[] words = line.Split('|');

            //There should be 5 elements on the line.
            if (words.Length != 5)
            {
                return null;
            }

            string currencyCode = words[3];

            //Currency code has length of 3.
            if (currencyCode.Length != 3)
            {
                return null;
            }

            //Checks if 'amount' is in a valid format.
            if (decimal.TryParse(words[2], out decimal amount) == false)
            {
                return null;
            }

            string textRate = words[4].Replace('.', ',');

            //Checks if 'rate' is in a valid format.
            if (decimal.TryParse(textRate, out decimal rate) == false)
            {
                return null;
            }

            return new ExchangeRate(new Currency(currencyCode), new Currency("CZK"), rate / amount);
        }
    }
}

