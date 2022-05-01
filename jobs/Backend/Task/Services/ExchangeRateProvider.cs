using CsvHelper;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger _logger;
        private IHttpClientFactory _httpFactory { get; set; }

        public ExchangeRateProvider(IHttpClientFactory httpFactory, ILogger logger)
        {
            _httpFactory = httpFactory;
            _logger = logger;
        }


        /// <inheritdoc/>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRates = new List<ExchangeRate>();

            var client = _httpFactory.CreateClient("HttpClientCzechNationalBank");

            _logger.LogInformation("Getting data from CNB api at {dateTime}", DateTime.UtcNow);
            var content = await client.GetStringAsync("");

            // remove first 1 lines (date of request) 
            var lines = content.Split(Environment.NewLine.ToCharArray()).Skip(1).ToArray();
            var cleaned = string.Join(Environment.NewLine, lines);

            List<ExchangeRate> result = new();
            using (var reader = new StringReader(cleaned))
            using (var csv = new CsvReader(reader, configuration: new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "|", HasHeaderRecord = true }))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new ExchangeRate(new Currency(csv.GetField("Code")), new Currency("CZK"), csv.GetField<decimal>("Rate"));
                    result.Add(record);
                }
                exchangeRates.AddRange(result.Where(w => currencies.Contains(w.SourceCurrency)));
            }

            return exchangeRates;
        }
    }
}
