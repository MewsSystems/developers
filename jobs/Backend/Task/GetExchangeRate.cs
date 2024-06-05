using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public  class GetExchangeRate
    {
        // this is the header of the data that we get from the bank
        // start reading from here all the way down ......
        private const string dataHeader = "Country|Currency|Amount|Code|Rate";
        private const char dataSeparator = '|';
        private const int expectedColumns = 5; // based on the current dataHeader
        private const string targetCurrency = "CZK"; // the currency we are interested in


        private readonly string _dataFromBank;
        private string DataFromBank { get => _dataFromBank; }

        public GetExchangeRate(string dataFromBank)
        {
            _dataFromBank = dataFromBank;
        }

        /// <summary>
        /// Will split the input data at new-line character
        /// and then parse the data into ExchangeRate objects.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ExchangeRate> ParseBankResponse()
        {
            if (string.IsNullOrWhiteSpace(DataFromBank))
                return Enumerable.Empty<ExchangeRate>();

            string[] allRates = DataFromBank.Split('\n', StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);
            bool startProcessing = false;
            
            List<ExchangeRate> ratesFromData = new List<ExchangeRate>();

            // we will start porcessing from the dataHeader downwards :
            foreach (string currentDataRow in allRates)
            {
                if (startProcessing)
                {
                    ExchangeRate exchangeRate = ParseDataRow(currentDataRow);
                    if (exchangeRate != null)
                    {
                        ratesFromData.Add(exchangeRate);
                    }
                }
                else if (currentDataRow == dataHeader)
                {
                    startProcessing = true;
                    continue;
                }
            }

            return ratesFromData;
        }

        /// <summary>
        /// Validate the data in the row and if valid create ExchangeRate object.
        /// </summary>
        /// <param name="currentRow"></param>
        /// <returns></returns>
        private ExchangeRate ParseDataRow(string currentRow)
        {
            if (string.IsNullOrWhiteSpace(currentRow))
            {
                Console.WriteLine("Empty row in the data.");
                return null;
            }

            // header columns :
            //   0       1       2       3    4
            // Country|Currency|Amount|Code|Rate
            string[] columns = currentRow.Split(dataSeparator);

            // validate the number of columns and the data in the columns
            if (columns.Length != expectedColumns)
            { 
                Console.WriteLine("Invalid number of columns in the data.");
                return null;
            }

            if (!decimal.TryParse(columns[4], out decimal rate))
            {
                Console.WriteLine("Invalid rate in the data.");
                return null;
            }

            if (!int.TryParse(columns[2], out int amount))
            {
                Console.WriteLine("Invalid amount in the data.");
                return null;
            }

            string code = columns[3];
            if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
            { 
                Console.WriteLine("Invalid currency code in the data.");
                return null;
            }

            // calculate the rate value based on the amount (as in case of yen - Japan or lira - Turkey)
            // where the exchange rate is for 100 units of the currency
            decimal rateValue = rate / (decimal)amount;
            return new ExchangeRate(new Currency(code), new Currency(targetCurrency), rateValue);
        }
    }
}
