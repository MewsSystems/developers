using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly string CNB_URL_DAILY;
        private readonly string CNB_URL_OTHER_RATES;
        private const char DELIMITER = '|';
        private const byte AMOUNT_COLUMN = 2;
        private const byte CODE_COLUMN = 3;
        private const byte RATE_COLUMN = 4;
        private readonly Currency _targetCurrency;

        public ExchangeRateProvider(IConfiguration configuration)
        {
            _targetCurrency = new Currency("CZK");
            CNB_URL_DAILY = configuration["ExchangeRateProvider:CnbUrlDaily"];
            CNB_URL_OTHER_RATES = configuration["ExchangeRateProvider:CnbUrlOtherRates"];
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var daily = GetExchangeRatesFromUrlAsync(currencies, CNB_URL_DAILY);
            var otherRates = GetExchangeRatesFromUrlAsync(currencies, CNB_URL_OTHER_RATES);

            await Task.WhenAll(daily, otherRates);

            return daily.Result.Concat(otherRates.Result);
        }

        private async Task<IEnumerable<ExchangeRate>> GetExchangeRatesFromUrlAsync(IEnumerable<Currency> currencies, string url)
        {
            List<ExchangeRate> result = new List<ExchangeRate>();

            using (var client = new HttpClientAdapter())
            {
                using var reader = new StreamReader(await client.GetStreamAsync(url));
                await SkipFileHeaderAsync(reader);

                string fileLine;
                while ((fileLine = await reader.ReadLineAsync()) != null)
                {
                    string[] commaSeparatedLine = fileLine.Split(DELIMITER);

                    if (currencies.Select(c => c.Code).Contains(commaSeparatedLine[CODE_COLUMN]))
                    {
                        if (decimal.TryParse(commaSeparatedLine[AMOUNT_COLUMN], out decimal amount)
                            && decimal.TryParse(commaSeparatedLine[RATE_COLUMN], out decimal rate))
                        {
                            result.Add(new ExchangeRate(new Currency(commaSeparatedLine[CODE_COLUMN]), _targetCurrency, rate / amount));
                        }
                    }
                }
            }
            return result;
        }

        private async Task SkipFileHeaderAsync(StreamReader reader)
        {
            await reader.ReadLineAsync();
            await reader.ReadLineAsync();
        }
    }
}
