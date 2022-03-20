using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using ExchangeRateUpdater.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Parse
{
    public class ExchangeRatesCnbCzParser : IExchangeRatesParser
    {
        private readonly Config appConfig;
        private readonly ILogger<ExchangeRatesCnbCzParser> logger;

        public ExchangeRatesCnbCzParser(Config appConfig, ILogger<ExchangeRatesCnbCzParser> logger)
        {
            this.appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<ExchangeRate> ParseRates(string exchangeRatesTxt)
        {
            if (string.IsNullOrWhiteSpace(exchangeRatesTxt))
            {
                logger.LogError("Tried to parse from null or empty string");
                throw new ArgumentException($"{nameof(exchangeRatesTxt)} should contain a valid string");
            }

            using var stringReader = new StringReader(exchangeRatesTxt);
            stringReader.ReadLine(); // skipping date

            var csvConfiguration = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = appConfig.ExchangeRateFieldsDelimeter
            };
            using var csvReader = new CsvReader(stringReader, csvConfiguration);

            try
            {
                var rates = csvReader.GetRecords<CnbCzExchangeRate>();
                return rates.Select(CnbCzRateToCurrency).ToList();
            }
            catch (CsvHelperException ex)
            {
                logger.LogError("Error while parsing csv: {message}", ex.Message);
                throw;
            }
        }

        private ExchangeRate CnbCzRateToCurrency(CnbCzExchangeRate cnbCzExchangeRate)
        {
            return new ExchangeRate(
                sourceCurrency: new Currency(cnbCzExchangeRate.Code),
                targetCurrency: new Currency(appConfig.ExchangeRatesForCurrency),
                value: cnbCzExchangeRate.Rate / cnbCzExchangeRate.Amount);
        }
    }
}
