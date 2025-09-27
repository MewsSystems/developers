using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Core.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateProvider _provider;
    private readonly IExchangeRateCache _cache;
    private readonly ILogger<ExchangeRateService> _logger;
    private readonly ExchangeRateOptions _options;

    public ExchangeRateService(
        IExchangeRateProvider provider,
        IExchangeRateCache cache,
        ILogger<ExchangeRateService> logger,
        IOptions<ExchangeRateOptions> options)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(
        IEnumerable<Currency> currencies,
        Maybe<DateTime> date
    )
    {
        if (currencies == null)
            throw new ArgumentNullException(nameof(currencies));

        var currencyList = currencies.ToList();
        if (!currencyList.Any())
            return Enumerable.Empty<ExchangeRate>();

        var targetDate = date.GetValueOrDefault(DateTime.Today);
        _logger.LogInformation($"Getting exchange rates for {currencyList.Count} currencies ({string.Join(", ", currencyList.Select(c => c.Code))}) for date {targetDate:yyyy-MM-dd}");

        try
        {
            if (_options.EnableCaching)
            {
                var cachedRates = await _cache.GetCachedRates(currencyList, targetDate);
                if (cachedRates.HasValue)
                {
                    _logger.LogInformation($"Returning {cachedRates.Value.Count()} cached exchange rates");
                    return cachedRates.Value;
                }
            }

            // Fetch from provider
            _logger.LogInformation($"Fetching fresh exchange rates from {_provider.ProviderName}");
            var maybeRates = await _provider.GetExchangeRatesForDate(date);

            if (maybeRates.TryGetValue(out var rateList))
            {
                if (rateList.Any())
                {
                    if (_options.EnableCaching)
                    {
                        await _cache.CacheRates(rateList, _options.DefaultCacheExpiry);
                    }
                    
                    _logger.LogInformation($"Successfully retrieved {rateList.Count()} exchange rates");
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
    }
}

public class ExchangeRateServiceException : Exception
{
    public ExchangeRateServiceException(string message) : base(message) { }
    public ExchangeRateServiceException(string message, Exception innerException) : base(message, innerException) { }
}
