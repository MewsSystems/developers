using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.CNB;
using ExchangeRateUpdater.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateSources.CNB;

public sealed class CNBExchangeRateSource : IExchangeRateSource
{
    private readonly IOptions<CNBSourceOptions> _options;
    private readonly ILogger<CNBExchangeRateSource> _logger;
    private readonly object _locker = new();

    private bool loaded = false;

    public CNBExchangeRateSource(IOptions<CNBSourceOptions> options, ILogger<CNBExchangeRateSource> logger)
    {
        _options = options;
        _logger = logger;
    }

    private ExchangeRateCache ExchangeRateCache { get; } = new();

    public async Task LoadAsync()
    {
        var source = CNBSourceFactory.CreateSource(_options.Value);
        string cnbSource = await source.GetContent();
        lock (_locker)
        {
            loaded = false;
            ExchangeRateCache.Clear();
            foreach (var rate in CNBExchangeRateParser.ParseRates(cnbSource))
            {
                ExchangeRateCache.Add(rate);
            }
            loaded = true;
        }
        LogResult(source);
    }

    public IEnumerable<ExchangeRate> GetSourceExchangeRates(Currency currency)
    {
        
        lock (_locker)
        {
            if (!loaded)
            {
                _logger.LogWarning("Requesting SourceExchangeRates but this source was not loaded");
            }
            if (ExchangeRateCache.SourceExchangeRates.TryGetValue(currency, out var exchangeRates))
            {
                return exchangeRates;
            };
        }
        return Enumerable.Empty<ExchangeRate>();
    }

    public IEnumerable<ExchangeRate> GetTargetExchangeRates(Currency currency)
    {
        
        lock (_locker)
        {
            if (!loaded)
            {
                _logger.LogWarning("Requesting TargetExchangeRates but this source was not loaded");
            }
            if (ExchangeRateCache.TargetExchangeRates.TryGetValue(currency, out var exchangeRates))
            {
                return exchangeRates;
            };
        }
        return Enumerable.Empty<ExchangeRate>();
    }



    private void LogResult(Sources.ISource source)
    {
        if (ExchangeRateCache.Count == 0)
        {
            _logger.LogWarning("Failed to load any exchange rates from source {source}", source);
        }
        else
        {
            _logger.LogInformation("Loaded {exchangeRateCount} exchange rates", ExchangeRateCache.Count);
        }
    }
}
