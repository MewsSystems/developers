using ExchangeRateUpdater.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try
            {
                string rawCurrencies = GetCNBCurrencyRates();
                if (string.IsNullOrEmpty(rawCurrencies))
                    return Enumerable.Empty<ExchangeRate>();

                return SplitDataAndFilter(rawCurrencies, currencies);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
                throw;
            }
        }
        /// <summary>
        /// Splits raw data line by line, and checks does it exist in currencies input,
        /// Passes the currency line if it's nonexistent 
        /// If exists, convert the raw line to the CurrencyItem to manage it easier.
        /// Adds to list of ExchangeRate, to return.
        /// </summary>
        /// <param name="rawCurrencies">Raw currency data whick is returned from GetCNBCurrencyRates()</param>
        /// <param name="currencies">Expected currency rates which is added from the caller method as param</param>
        /// <returns></returns>
        public List<ExchangeRate> SplitDataAndFilter(string rawCurrencies, IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> result = new List<ExchangeRate>();
            try
            {
                StringReader reader = new StringReader(rawCurrencies);

                string currencyLine;
                while ((currencyLine = reader.ReadLine()) != null)
                {
                    //Expected format (AppDomain.config) Country | Currency | Amount | Code | Rate
                    string[] values = currencyLine.Split('|');
                    if (currencies.Where(a => a.Code == values[3]).Count() > 0)
                    {
                        CurrencyItem tempCurrency = new CurrencyItem()
                        {
                            Country = values[0],
                            Currency = values[1],
                            Amount = int.Parse(values[2].ToString((CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture),
                            Code = values[3],
                            Rate = decimal.Parse(values[4].ToString((CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture)
                        };
                        result.Add(new ExchangeRate(
                            new Currency(tempCurrency.Code),
                            new Currency("CZK"),
                            Math.Round(tempCurrency.SingleVal, 4))
                            );
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An unexpected error occured: '{e.Message}'.");
                throw;
            }
            return result;
        }
        /// <summary>
        /// Gets currency rates from CNB API.
        /// API URL idendified in app.config with APIUrl key. You can check or change from there.
        /// Returns a string raw data with format check DataFormatVerification method for the validation.
        /// </summary>
        /// <returns>String raw data</returns>
        public string GetCNBCurrencyRates()
        {
            string result = null;
            try
            {              
                WebClient client = new WebClient();
                string url = ConfigurationSettings.AppSettings.Get("APIUrl");
                byte[] raw = client.DownloadData(url);
                string rawData = System.Text.Encoding.UTF8.GetString(raw);

                bool checkFormat = DataFormatVerification(rawData.Split(Environment.NewLine.ToCharArray(), 3)[1]);
                if (!checkFormat)
                {
                    Console.WriteLine("Raw data is not in the expected format");
                    return null;
                }

                //First line is DateTime
                //Second line is Format
                //So remove first 2 lines to abtain the useful data only.
                result = rawData.Split(Environment.NewLine.ToCharArray(), 3).Skip(2).FirstOrDefault();

                return result;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"Could not retrieve exchange rates, server return NULL: '{e.Message}'.");
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine($"An unexpected error occured, try to restart your pc :P or let us know about the error: '{e.Message}'.");
                throw;
            }
        }
        /// <summary>
        /// The bank API returns a dataset.
        /// With that method, we can verify the expected data's format.
        /// If something wrong, we should not try to process data, so method returns False
        /// Checks input data with the base format from app.config -> APIFormat key
        /// </summary>
        /// <param name="format">String format from the second line of the obtained data of the CNB</param>
        /// <returns>True/False</returns>
        public bool DataFormatVerification(string format)
        {
            string baseFormat = ConfigurationSettings.AppSettings.Get("APIFormat");
            if (format.Equals(baseFormat))
                return true;
            else
                return false;
        }
    }
}
