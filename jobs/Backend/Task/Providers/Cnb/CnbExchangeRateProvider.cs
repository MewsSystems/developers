using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services.Providers.Cnb
{
    /// <summary>
    /// Will get exchange rates from CNB - Czech National Bank
    /// </summary>
    internal class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private const string CNB_CLIENT_NAME = "CnbExchangeRateHttpClient";
        private const string CNB_API = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=";
        private const string PROVIDER_NAME = "CNB";
        
        public string Name => PROVIDER_NAME;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CnbCsvParser _cnbCsvParser;

        public CnbExchangeRateProvider(
            IHttpClientFactory httpClientFactory
            )
        {
            _httpClientFactory = httpClientFactory;
            _cnbCsvParser = new();
        }
        
        /// <inheritdoc />
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var currentDate = DateTime.Today;
            var apiUrl = $"{CNB_API}{currentDate:dd.MM.yyyy}";
            var httpClient = _httpClientFactory.CreateClient(CNB_CLIENT_NAME);
            httpClient.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Text.Plain);

            try
            {
                await using (var responseStream = await httpClient.GetStreamAsync(apiUrl))
                {
                    var rates = _cnbCsvParser
                        .ParseExchangeRates(responseStream)
                        .Where(x => currencies.Any(source => source.Code == x.SourceCurrency.Code))
                        .ToList();
                    return rates;
                }
            }
            catch (Exception e)
            {
                Console.Write("GetExchangeRatesAsync not succeed!", e);
                throw;
            }
        }
    }
}