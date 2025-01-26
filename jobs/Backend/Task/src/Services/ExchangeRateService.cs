using System;
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

        public ExchangeRateService(
            IExchangeRateProvider provider,
            ILogger<ExchangeRateService> logger,
            IOptions<ExchangeRateOptions> options,
            IHostApplicationLifetime appLifetime)
        {
            _provider = provider;
            _logger = logger;
            _options = options.Value;
            _appLifetime = appLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var currencies = _options.CurrenciesToWatch.Select(code => new Currency(code));
                var rates = await _provider.GetExchangeRatesAsync(currencies);

                _logger.LogInformation("Successfully retrieved {Count} exchange rates", rates);
                foreach (var rate in rates)
                {
                    _logger.LogInformation("Exchange rate: {Rate}", rate);
                }

                // Solicitar el cierre de la aplicación
                _appLifetime.StopApplication();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not retrieve exchange rates");
                throw; // Re-throw to indicate service startup failure
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
} 