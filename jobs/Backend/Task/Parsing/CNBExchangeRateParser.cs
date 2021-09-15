using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ExchangeRateUpdater.Parsing
{
    public class CNBExchangeRateParser : IExchangeRateParser
    {
        private const string BASE_CURRENCY = "CZK";
        public IEnumerable<ExchangeRate> Parse(string rawData)
        {
            var rates = new List<ExchangeRate>();

            if (string.IsNullOrEmpty(rawData))
            {
                return rates;
            }

            using (var reader = new StringReader(rawData))
            {

               var dateString = reader.ReadLine();
                if (string.IsNullOrEmpty(dateString))
                {
                    Trace.TraceError("Exchange rate data in an unexpected format: exchange rate date missing.");
                    return rates;
                }

                var headers = reader.ReadLine();
                if (string.IsNullOrEmpty(dateString))
                {
                    Trace.TraceError("Exchange rate data in an unexpected format: headers missing.");
                    return rates;
                }

                var line = 2;

                while (true)
                {
                    line++;
                    var dataLine = reader.ReadLine();

                    if(dataLine == null)
                    {
                        break;
                    }

                    if(dataLine == string.Empty)
                    {
                        continue;
                    }

                    try
                    {
                        rates.Add(ParseLine(dataLine));
                    }
                    catch (ArgumentException ex)
                    {
                        Trace.TraceError($"Failed to parse line {line} - {ex.Message}");
                    }
                }
            }

            return rates;
        }

        private ExchangeRate ParseLine(string dataLine)
        {
            var components = dataLine.Split('|');
            if(components.Length < 5)
            {
                throw new ArgumentException("Exchange rate data in an unexpected format.");
            }

            var currency = components[3];
            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentException("Exchange rate data in an unexpected format - source currency cannot be determined.");
            }

            if(!decimal.TryParse(components[2], out decimal amount))
            {
                throw new ArgumentException("Exchange rate data in an unexpected format - amount cannot be determined.");
            }

            if (!decimal.TryParse(components[4], out decimal rate))
            {
                throw new ArgumentException("Exchange rate data in an unexpected format - rate cannot be determined.");
            }

            return new ExchangeRate(new Currency(currency), new Currency(BASE_CURRENCY), rate, amount);
        }
    }
}