using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Interfaces;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly Dictionary<string, (decimal Amount, decimal Rate)> _rates = new();
        private readonly IExchangeRateDataProvider _dataProvider;
        private readonly IExchangeRateParser _parser;
        private readonly ExchangeRateOptions _options;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProvider(
            IExchangeRateDataProvider dataProvider,
            IExchangeRateParser parser,
            IOptions<ExchangeRateOptions> options,
            ILogger<ExchangeRateProvider> logger)
        {
            _dataProvider = dataProvider;
            _parser = parser;
            _options = options.Value;
            _logger = logger;

            _logger.LogInformation("Initializing exchange rate provider...");
            UpdateRatesAsync().Wait(); // In production, this should be handled asynchronously
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            _logger.LogDebug("Starting method GetExchangeRatesAsync with {CurrencyCount} currencies.", currencies.Count());
    
            var result = new List<ExchangeRate>();
            var baseCurrency = new Currency(_options.BaseCurrency);
            _logger.LogDebug("Base currency set to {BaseCurrency}.", baseCurrency.Code);

            var missingCurrencies = new List<string>();

            foreach (var currency in currencies)
            {
                _logger.LogDebug("Processing currency: {CurrencyCode}.", currency.Code);

                if (_rates.TryGetValue(currency.Code, out var rateInfo))
                {
                    _logger.LogDebug("Exchange rate found for {CurrencyCode}. Rate: {Rate}, Amount: {Amount}.", 
                        currency.Code, rateInfo.Rate, rateInfo.Amount);

                    var exchangeRate = new ExchangeRate(
                        baseCurrency,
                        currency,
                        rateInfo.Rate / rateInfo.Amount
                    );
                    result.Add(exchangeRate);
                }
                else
                {
                    _logger.LogDebug("Exchange rate not found for {CurrencyCode}.", currency.Code);
                    missingCurrencies.Add(currency.Code);
                }
            }

            if (missingCurrencies.Any())
            {
                _logger.LogInformation("Exchange rates not found for the following currencies: {MissingCurrencies}",
                    string.Join(", ", missingCurrencies));
            }

            _logger.LogInformation("Completed method GetExchangeRatesAsync. Processed {ProcessedCount} currencies with {MissingCount} missing rates.",
                result.Count, missingCurrencies.Count);

            return result;
        }

        public async Task UpdateRatesAsync()
        {
            try
            {
                _logger.LogInformation("Updating exchange rates...");
                var data = await _dataProvider.GetRawDataAsync();
                var parsedRates = _parser.ParseRates(data);

                _rates.Clear();
                foreach (var (code, amount, rate) in parsedRates)
                {
                    _rates[code] = (amount, rate);
                }

                _logger.LogDebug("Exchange rates updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update exchange rates");
                throw;
            }
        }
    }
} 