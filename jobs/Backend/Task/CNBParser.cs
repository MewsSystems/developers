using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class CNBParser : IRateParser
    {
        public string ForHost() => "www.cnb.cz";

        /// <summary>
        /// Provided source is first checked for basic correctness (its not empty, it has actually rows). 
        /// Secondary, source from CNB contains metadata on first two rows, date is logged to indicate for which day these rates apply.
        /// Rest of the source are lines containing one rate per row. 
        /// At the moment, parser is interested in target currency code and also in the value due to how source currency is determined.
        /// </summary>
        public IEnumerable<ExchangeRate> ParseSource(string source)
        {
            // If source doesn't contain anything, there are no available Exchange Rates.
            if(string.IsNullOrWhiteSpace(source))
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            string[] parsedToLines = source.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            int numberOfFields = HandleMetadataAndValidate(parsedToLines);

            List<ExchangeRate> rates = new List<ExchangeRate>();

            // Starting from row 2, first two rows are metadata.
            for(int i = 2; i < parsedToLines.Count(); i++)
            {
                ExchangeRate newRate = CreateNewRateFromParsedLine(parsedToLines[i], numberOfFields);
                if(newRate == null)
                {
                    continue;
                }
                rates.Add(newRate);
            }

            return rates;
        }

        /// <summary>
        /// Source information may be malformed, for example, missing line breaks or no columns specified. 
        /// Metada at the start of the source is used to report day when rates were "captured"
        /// and number of columns is returned to allow individual row validation.
        /// </summary>
        private int HandleMetadataAndValidate(string[] parsedToLines)
        {
            if(parsedToLines.Count() == 0)
            {
                throw new FormatException("There are issues with the source format from cnb.cz source. Failed to parse lines and rows in the source file.");
            }

            string[] dayAndNumber = parsedToLines[0].Split("#");
            if(dayAndNumber.Count() > 1)
            {
                Console.WriteLine($"Retrieved list of Exchange Rates for day: {dayAndNumber[0]}");
            }

            // At least one column needs to be specified, column names can't be empty.
            string[] columnNames = parsedToLines[1].Split("|", StringSplitOptions.RemoveEmptyEntries);
            if(columnNames.Count() == 0)
            {
                throw new FormatException("There are issues with the source format from cnb.cz source. There are no columns specified.");
            }
            return columnNames.Count();
        }

        /// <summary>
        /// Validate parsed line and if everything is correct, create new rate.
        /// Rules:
        /// - row has to have specified number of fields, based on column names from metadata.
        /// - there are two fields required, Code and Value, fourth and fifth field.
        /// - value needs to be parseable to decimal, a number. Either integer or float.
        /// Creation of new rate works on basis of only what is correct, makes it through, rest is ignored. No exceptions are thrown.
        /// If row doesn't have necessary information, it is skipped in the for cycle calling this method.
        /// </summary>
        private ExchangeRate CreateNewRateFromParsedLine(string parsedLine, int numberOfFields)
        {
                string[] parsedRate = parsedLine.Split("|");
                if(parsedRate.Count() != numberOfFields)
                {
                    Console.WriteLine($"Parsed Exchange Rate row doesn't contain appropriate number of fields: {parsedRate.Count()}. Should have {numberOfFields}. Skipping.");
                    return null;
                }
                if(parsedRate.Count() < 5)
                {
                    Console.WriteLine($"Parsed Exchange Rate row doesn't contain required fields: Code and/or Value. Skipping. Row: {parsedLine}");
                    return null;
                }

                string targetCurrencyCode = parsedRate[3];
                if(string.IsNullOrWhiteSpace(targetCurrencyCode))
                {
                    Console.WriteLine($"Parsed Exchange Rate row doesn't contain currency Code, skipping. Row: {parsedLine}");
                    return null;
                }

                string parsedValue = parsedRate[4];
                if(string.IsNullOrWhiteSpace(parsedValue))
                {
                    Console.WriteLine($"Parsed Exchange Rate row doesn't contain Rate value, skipping. Row: {parsedLine}");
                    return null;
                }

                decimal rateValue = 0;
                try
                {
                    rateValue = decimal.Parse(parsedValue);
                }
                catch
                {
                    Console.WriteLine($"Parsed Exchange Rate row contains Value that is not number, skipping. Row: {parsedLine}");
                    return null;
                }

                return new ExchangeRate(new Currency("CZK"), new Currency(targetCurrencyCode), rateValue);
        }
    }
}