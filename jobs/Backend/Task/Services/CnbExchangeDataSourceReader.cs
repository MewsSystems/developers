using ExchangeRateProvider.Models;
using ExchangeRateUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Services
{
    public class CnbExchangeDataSourceReader : BaseExchangeDataSourceReader
    {
        /// <summary>
        /// Fetches and parses the exchange rates from the data source.
        /// </summary>
        /// <param name="baseExchangeDataSource"></param>
        /// <returns></returns>
        public override async Task<List<ExchangeRate>> FetchAndParseExchangeRatesAsync(BaseExchangeDataSource baseExchangeDataSource)
        {
            _baseExchangeDataSource = baseExchangeDataSource;

            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(baseExchangeDataSource.GetExchangeRateDatasetUrl());

            return ParseExchangeRates(response);
        }

        /// <summary>
        /// Parses the exchange rates from the text data.
        /// </summary>
        /// <param name="textData"></param>
        /// <returns></returns>
        private List<ExchangeRate> ParseExchangeRates(string textData)
        {
            var exchangeRates = new List<ExchangeRate>();
            var lines = textData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);


            // Skip the first two lines (date and header)
            foreach (var line in lines.Skip(2))
            {
                var parts = line.Split('|');
                if (parts.Length >= 5)
                {
                    var currencyCode = parts[3];
                    var amount = int.Parse(parts[2]); // Amount indicates the base unit (e.g., 100 HUF)
                    if (decimal.TryParse(parts[4], out var rate))
                    {
                        var sourceCurrency = new Currency(_baseExchangeDataSource.SourceCurrencyCode.ToString());
                        var targetCurrency = new Currency(currencyCode);
                        exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, rate / amount));
                    }
                }
            }
            return exchangeRates;
        }
    }
}
