using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateService : IHostedService
    {
        private readonly IExchangeRateProvider _provider;
        private readonly ILogger<ExchangeRateService> _logger;
        private readonly ExchangeRateOptions _options;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ICurrencyIsoService _currencyIsoService;

        public ExchangeRateService(
            IExchangeRateProvider provider,
            ILogger<ExchangeRateService> logger,
            IOptions<ExchangeRateOptions> options,
            IHostApplicationLifetime appLifetime, ICurrencyIsoService currencyIsoService)
        {
            _provider = provider;
            _logger = logger;
            _options = options.Value;
            _appLifetime = appLifetime;
            _currencyIsoService = currencyIsoService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting service...");

            try
            {
                var currencies = new List<Currency>();
                _logger.LogDebug("Validating currency codes from configuration.");

                foreach (var currencyCode in _options.CurrenciesToWatch)
                {
                    try
                    {
                        _currencyIsoService.ValidateCode(currencyCode);
                        currencies.Add(new Currency(currencyCode));
                        _logger.LogDebug("Currency code {CurrencyCode} validated and added.", currencyCode);
                    }
                    catch (ArgumentException ex)
                    {
                        _logger.LogWarning("Invalid currency code: {CurrencyCode}. Exception: {Message}", currencyCode, ex.Message);
                    }
                }

                _logger.LogDebug("Validated {CurrencyCount} currencies. Fetching exchange rates...", currencies.Count);

                var rates = await _provider.GetExchangeRatesAsync(currencies);

                _logger.LogInformation("Retrieved {RateCount} exchange rates.", rates.Count());

                foreach (var rate in rates)
                {
                    _logger.LogInformation("Exchange rate: Base={BaseCurrency}, Target={TargetCurrency}, Rate={Rate}", 
                        rate.SourceCurrency.Code, rate.TargetCurrency.Code, rate.Value);
                }

                _logger.LogInformation("Stopping application gracefully...");
                _appLifetime.StopApplication();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not retrieve exchange rates due to an unexpected error.");
                throw; // Re-throw to indicate service startup failure
            }
            finally
            {
                _logger.LogInformation("Service execution finished.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
} 