using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Fluent;
using ExchangeRateUpdater.Structures;

namespace ExchangeRateUpdater
{
    public static class ExchangeRateExtensions
    {
        public static async Task<List<ExchangeRate>> ToList(this IAsyncEnumerable<ExchangeRate> rates)
        {
            var resultRates = new List<ExchangeRate>(); 
            await foreach (var rate in rates)
            {
                resultRates.Add(rate);
            }

            return resultRates;
        }

        static decimal ReadAndParseDecimal(Table.Row row, string headerName)
        {
            if (!decimal.TryParse(row[headerName], out var value))
            {
                throw new Exception($"Value '{value}' in column '{headerName}' cannot be parsed to decimal");
            }

            return value;
        }
        
        public static async IAsyncEnumerable<ExchangeRate> ComputeExchangeRates(
            this Task<Fluent<Table>> fluentTableTask, IEnumerable<Currency> currencies)
        {
            var fluentTable = await fluentTableTask;

            var currencyCodes = currencies.Select(currency => currency.Code).ToHashSet();

            foreach (var row in fluentTable.Value.Rows)
            {
                var code = row[Constants.CodeHeaderName];
                var amount = ReadAndParseDecimal(row, Constants.AmountHeaderName);
                var rate = ReadAndParseDecimal(row, Constants.RateHeaderName);
                
                if (!currencyCodes.Contains(code))
                {
                    continue;
                }

                yield return new ExchangeRate(new Currency(code), new Currency("CZK"), 
                    rate / amount);
            }
        }
    }
}