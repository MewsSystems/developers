using ExchangeRateUpdater.Application.Configuration;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Application.Services;

public sealed class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateApiClient _apiClient;
    private readonly ExchangeRateConfig _exchangeRateConfig;
    private readonly ILogger<ExchangeRateProvider> _logger;

    public ExchangeRateProvider(
        IExchangeRateApiClient apiClient, 
        IOptions<ExchangeRateConfig> exchangeRateConfig,
        ILogger<ExchangeRateProvider> logger)
    {
        _apiClient = apiClient;
        _exchangeRateConfig = exchangeRateConfig.Value;
        _logger = logger;
    }

    public async Task<List<ExchangeRate>> GetExchangeRates(string[] currencies, CancellationToken cancellationToken)
    {
        var cnbExchangeRates = await _apiClient.GetAllExchangeRatesAsync(cancellationToken);
        return MapExchangeRates(cnbExchangeRates, currencies).ToList();
    }

    private IEnumerable<ExchangeRate> MapExchangeRates(
        IEnumerable<CnbExchangeRate> cnbExchangeRates,
        string[] currencyCodes)
    {
        var rateByCurrencyCode =
            cnbExchangeRates.ToDictionary(x => x.CurrencyCode, StringComparer.OrdinalIgnoreCase);
        foreach (var currencyCode in currencyCodes)
        {
            if (!rateByCurrencyCode.TryGetValue(currencyCode, out var exchangeRate))
            {
                _logger.LogWarning("No matches were found among CNB rates for currency '{@currencyCode}'", currencyCode);
                continue;
            }

            yield return new(
                exchangeRate.CurrencyCode,
                _exchangeRateConfig.DefaultCurrency,
                Math.Round(exchangeRate.Rate / exchangeRate.Amount, _exchangeRateConfig.RatePrecision));
        }
    }
}