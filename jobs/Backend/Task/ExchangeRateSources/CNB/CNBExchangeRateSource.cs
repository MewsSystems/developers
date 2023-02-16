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

    public CNBExchangeRateSource(IOptions<CNBSourceOptions> options, ILogger<CNBExchangeRateSource> logger)
    {
        _options = options;
        _logger = logger;
    }

    public IDictionary<(string sourceCode, string targetCode), ExchangeRate> ExchangeRates { get; }
        = new Dictionary<(string, string), ExchangeRate>();

    public async Task LoadAsync()
    {
        var source = CNBSourceFactory.CreateSource(_options.Value);
        string cnbSource = await source.GetContent();
        foreach (var rate in CNBExchangeRateParser.ParseRates(cnbSource))
        {
            ExchangeRates.Add((rate.SourceCurrency.Code, rate.TargetCurrency.Code), rate);
        }
        LogResult(source);
    }

    private void LogResult(Sources.ISource source)
    {
        if (!ExchangeRates.Any())
        {
            _logger.LogWarning("Failed to load any exchange rates from source {source}", source);
        }
        else
        {
            _logger.LogInformation("Loaded {exchangeRateCount} exchange rates", ExchangeRates.Count);
        }
    }
}
