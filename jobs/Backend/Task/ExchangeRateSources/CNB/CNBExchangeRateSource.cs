using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.CNB;
using ExchangeRateUpdater.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateSources.CNB
{
    public sealed class CNBExchangeRateSource : IExchangeRateSource
    {
        private readonly IOptions<CNBSourceOptions> _options;
        private readonly ILogger<CNBExchangeRateSource> _logger;
        private readonly object _locker = new();
        private bool loaded;

        private List<ExchangeRate> _exchangeRates = new();

        public CNBExchangeRateSource(IOptions<CNBSourceOptions> options, ILogger<CNBExchangeRateSource> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task LoadAsync()
        {
            var source = CNBSourceFactory.CreateSource(_options.Value);
            string cnbSource = await source.GetContent();
            lock (_locker)
            {
                loaded = false;
                _exchangeRates = new();
                foreach (var rate in CNBExchangeRateParser.ParseRates(cnbSource))
                {
                    _exchangeRates.Add(rate);
                }
                loaded = true;
            }
            LogResult(source);
        }

        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            lock (_locker)
            {
                if (!loaded)
                {
                    _logger.LogWarning("Requesting ExchangeRates but this source was not loaded");
                }
                return _exchangeRates.AsReadOnly();
            }
        }

        private void LogResult(Sources.ISource source)
        {
            if (_exchangeRates.Count == 0)
            {
                _logger.LogWarning("Failed to load any exchange rates from source {source}", source);
            }
            else
            {
                _logger.LogInformation("Loaded {exchangeRateCount} exchange rates", _exchangeRates.Count);
            }
        }
    }
}
