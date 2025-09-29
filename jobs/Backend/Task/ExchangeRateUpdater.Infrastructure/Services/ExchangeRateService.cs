using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Infrastructure.Telemetry;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateProvider _provider;
    private readonly IExchangeRateCache _cache;
    private readonly ILogger<ExchangeRateService> _logger;
    private readonly ExchangeRateOptions _exchangeOptions;

    public ExchangeRateService(
        IExchangeRateProvider provider,
        IExchangeRateCache cache,
        ILogger<ExchangeRateService> logger,
        IOptions<ExchangeRateOptions> options)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _exchangeOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(
        IEnumerable<Currency> currencies,
        Maybe<DateOnly> date
    )
    {
        using var activity = ExchangeRateTelemetry.ActivitySource.StartActivity("GetExchangeRates");
        activity?.SetTag("currency.count", currencies.Count());
        activity?.SetTag("date", date.ToString());

        if (currencies == null)
            throw new ArgumentNullException(nameof(currencies));

        var currencyList = currencies.ToList();
        if (!currencyList.Any())
            return Enumerable.Empty<ExchangeRate>();

        var targetDate = date.GetValueOrDefault(DateHelper.Today);
        _logger.LogInformation($"Getting exchange rates for {currencyList.Count} currencies ({string.Join(", ", currencyList.Select(c => c.Code))}) for date {targetDate:yyyy-MM-dd}");
        var cachedRates = Maybe<IReadOnlyList<ExchangeRate>>.Nothing;

        try
        {
            if (_exchangeOptions.EnableCaching)
            {
                cachedRates = await _cache.GetCachedRates(currencyList, targetDate);
                if (cachedRates.HasValue)
                {
                    _logger.LogInformation($"Returning {cachedRates.Value.Count()} cached exchange rates");
                    ExchangeRateTelemetry.CacheHits.Add(1, new KeyValuePair<string, object?>("currency.count", currencyList.Count));
                    return cachedRates.Value;
                }
                else
                {
                    ExchangeRateTelemetry.CacheMisses.Add(1, new KeyValuePair<string, object?>("currency.count", currencyList.Count));
                }
            }

            // Fetch from provider
            _logger.LogInformation($"Fetching fresh exchange rates from {_provider.ProviderName}");
            var maybeRates = await _provider.GetExchangeRatesForDate(date);

            if (maybeRates.TryGetValue(out var rateList))
            {
                if (rateList.Any())
                {
                    if (_exchangeOptions.EnableCaching)
                    {
                        await _cache.CacheRates(rateList);
                    }

                    _logger.LogInformation($"Successfully retrieved {rateList.Count()} exchange rates");
                    ExchangeRateTelemetry.ExchangeRateRequests.Add(1, new KeyValuePair<string, object?>("currency.count", currencyList.Count));
                    return rateList.Where(rate => currencyList.Contains(rate.SourceCurrency));
                }
                else
                {
                    _logger.LogWarning("No exchange rates found for the requested currencies");
                    return Enumerable.Empty<ExchangeRate>();
                }
            }
            else
            {
                _logger.LogWarning("Failed to retrieve exchange rates from provider");
                return Enumerable.Empty<ExchangeRate>();
            }
        }
        catch (ExchangeRateProviderException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while getting exchange rates");
            throw new ExchangeRateServiceException("An unexpected error occurred while getting exchange rates", ex);
        }
        finally
        {
            ExchangeRateTelemetry.ExchangeRateDuration.Record(
                activity?.Duration.TotalSeconds ?? 0,
                new KeyValuePair<string, object?>("source", cachedRates.HasValue ? "cache" : "provider")
            );
        }
    }
}

public class ExchangeRateServiceException : Exception
{
    public ExchangeRateServiceException(string message) : base(message) { }
    public ExchangeRateServiceException(string message, Exception innerException) : base(message, innerException) { }
}
