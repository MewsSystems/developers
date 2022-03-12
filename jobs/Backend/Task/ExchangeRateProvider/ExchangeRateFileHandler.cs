using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExchangeRateProvider
{
    /// <summary>
    /// This class handles all operations regarding cache file (Write, Read..)
    /// </summary>
    public class ExchangeRateFileHandler : IExchangeRateFileHandler
    {
        public string CachedFilePath { get; set; }

        /// <summary>
        /// Reads current exchange rates from cached file.
        /// </summary>
        public IDictionary<CurrencyCode, ExchangeRate> Read()
        {
            if (!File.Exists(CachedFilePath))
            {
                throw new FileNotFoundException();
            }

            var loadedExchangeRates = new Dictionary<CurrencyCode, ExchangeRate>();

            using (var parser = new TextFieldParser(CachedFilePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("|");

                while (!parser.EndOfData)
                {
                    try
                    {
                        string[] line = parser.ReadFields();
                        int amount = int.Parse(line[0]);
                        CurrencyCode sourceCurrency = new CurrencyCode(line[1]);
                        CurrencyCode targetCurrenty = new CurrencyCode(line[2]);
                        decimal rate = Convert.ToDecimal(line[3]);
                        ExchangeRate exchangeRate = new ExchangeRate(amount, sourceCurrency, targetCurrenty, rate);
                        loadedExchangeRates.Add(sourceCurrency, exchangeRate);
                    }
                    catch
                    {
                        // log exception
                        throw;
                    }
                }
            }

            return loadedExchangeRates;
        }

        /// <summary>
        /// Creates cache file with current exchange rates.
        /// </summary>
        /// <param name="exchangeRates">Hashtable with exchange rates.</param>
        public void Write(IDictionary<CurrencyCode, ExchangeRate> exchangeRates)
        {
            using (StreamWriter writer = new StreamWriter(CachedFilePath))
            {
                foreach (var exchangeRate in exchangeRates.Values)
                {
                    string line = $"{exchangeRate.Amount}|{exchangeRate.SourceCurrency}|{exchangeRate.TargetCurrency}|{exchangeRate.Rate}";
                    writer.WriteLine(line);
                    writer.Flush();
                }
            }
        }

        /// <summary>
        /// Verifies if cached file is up to date and contains correct exchange rates.
        /// Assuming that CNB releases new rates each day after 14:30 PM.
        /// According to their website these rates are valid until next day 14:30PM.
        /// </summary>
        /// <returns>True if rates up to date otherwise false.</returns>
        public bool IsCachedFileUpToDate()
        {
            if (!File.Exists(CachedFilePath))
            {
                return false;
            }

            DateTime timeRightNow = DateTime.Now;
            DateTime fileCreatedTime = File.GetCreationTime(CachedFilePath);

            // If file created before 14:30
            if (fileCreatedTime.TimeOfDay < new TimeSpan(14, 30, 0))
            {
                DateTime bottomThreshold = timeRightNow.Date.AddDays(-1) + new TimeSpan(14, 30, 0);
                DateTime upperThreshold = timeRightNow.Date + new TimeSpan(14, 30, 0);

                if (fileCreatedTime > bottomThreshold && fileCreatedTime < upperThreshold)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // if file created after 14:30
                DateTime bottomThreshold = timeRightNow.Date + new TimeSpan(14, 30, 0);
                DateTime upperThreshold = timeRightNow.Date + new TimeSpan(1, 14, 30, 0);

                if (fileCreatedTime > bottomThreshold && fileCreatedTime < upperThreshold)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
