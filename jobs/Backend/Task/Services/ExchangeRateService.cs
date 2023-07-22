using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateProvider _provider;
        private readonly ICurrencyLoader _currencyLoader;
        private readonly IOutputService _outputService;
        private readonly ILogger<IExchangeRateService> _logger;

        public ExchangeRateService(IExchangeRateProvider provider, ICurrencyLoader currencyLoader, ILogger<IExchangeRateService> logger, IOutputService outputService)
        {
            _provider = provider;
            _currencyLoader = currencyLoader;
            _logger = logger;
            _outputService = outputService;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var currencies = _currencyLoader.LoadCurrencies();
                var ratesFromApi = await _provider.GetExchangeRatesAsync(currencies, DateTime.Now);
                PrintRates(ratesFromApi, "API");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing JSON data");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Currency data file not found");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving exchange rates");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
            }
        }

        private void PrintRates(IEnumerable<ExchangeRate> rates, string source)
        {
            var message = $"Successfully retrieved {rates.Count()} exchange rates from {source}:";
            _outputService.WriteMessage(message);
            foreach (var rate in rates)
            {
                _outputService.WriteMessage(rate.ToString());
            }
        }
    }
}
