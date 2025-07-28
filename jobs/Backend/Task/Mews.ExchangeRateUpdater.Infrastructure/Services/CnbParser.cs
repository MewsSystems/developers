using Mews.ExchangeRateUpdater.Domain.ValueObjects;
using Mews.ExchangeRateUpdater.Infrastructure.Dtos;
using Mews.ExchangeRateUpdater.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Mews.ExchangeRateUpdater.Infrastructure.Services;

public class CnbParser : ICnbParser
{
    private readonly ILogger<CnbParser> _logger;

    public CnbParser(ILogger<CnbParser> logger)
    {
        _logger = logger;
    }

    public IEnumerable<ExchangeRate> Parse(CnbResponse? response)
    {
        if (response is null)
            return Enumerable.Empty<ExchangeRate>();
        
        if (response?.Rates == null || !response.Rates.Any())
        {
            _logger.LogWarning("Empty or null response received from CNB.");
            return Enumerable.Empty<ExchangeRate>();
        }

        var result = new List<ExchangeRate>();

        foreach (var r in response.Rates)
        {
            if (r.Amount <= 0 || string.IsNullOrWhiteSpace(r.CurrencyCode))
            {
                _logger.LogWarning("Invalid rate entry skipped: {@Entry}", r);
                continue;
            }

            try
            {
                var normalized = r.Rate / r.Amount;
                result.Add(new ExchangeRate(
                    new Currency(r.CurrencyCode),
                    new Currency("CZK"),
                    normalized
                ));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error processing entry: {@Entry}", r);
            }
        }

        return result;
    }
}
