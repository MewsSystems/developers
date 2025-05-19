using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Common.Models;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using ExchangeRateUpdater.Core.Interfaces;
using Microsoft.Extensions.Logging;
using NodaTime;
using NodaTime.Text;

namespace ExchangeRateUpdater.Infrastructure.Services;

public class CnbExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateDataSource _dataSource;
    private readonly ICacheService _cacheService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<CnbExchangeRateService> _logger;
    private const string CzechCurrencyCode = "CZK";

    public CnbExchangeRateService(
        IExchangeRateDataSource dataSource,
        ICacheService cacheService,
        IDateTimeProvider dateTimeProvider,
        ILogger<CnbExchangeRateService> logger)
    {
        _dataSource = dataSource;
        _cacheService = cacheService;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<ExchangeRateData> GetExchangeRatesAsync(LocalDate date,
        CancellationToken cancellationToken = default)
    {
        // Try to get from cache first
        var cachedData = await _cacheService.GetExchangeRatesAsync(date, cancellationToken);
        if (cachedData != null)
        {
            _logger.LogInformation("Retrieved exchange rates from cache for date {Date}", date);
            return cachedData;
        }

        // Otherwise fetch from data source and cache
        _logger.LogInformation("Fetching exchange rates from data source for date {Date}", date);
        var data = await _dataSource.GetExchangeRatesAsync(date, cancellationToken);

        // Determine published date - simple check if it's a working day
        LocalDate publishedDate;
        if (!_dateTimeProvider.IsWorkingDay(date))
        {
            // If not a working day, find the most recent working day
            publishedDate = _dateTimeProvider.GetLastWorkingDay(date);
            data = data with { PublishedDate = publishedDate };
            _logger.LogInformation("Date {Date} is not a working day, using published date {PublishedDate}",
                date,
                publishedDate);
        }

        // Cache the result
        await _cacheService.SetExchangeRatesAsync(date, data, cancellationToken);

        return data;
    }


    public async Task<ExchangeRateResponse> GetExchangeRateAsync(
        string sourceCurrency,
        string targetCurrency,
        LocalDate? date = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var targetDate = date ?? _dateTimeProvider.GetCurrentDate();

            // Get all rates for the specified date
            var ratesData = await GetExchangeRatesAsync(targetDate, cancellationToken);
            var dateStr = targetDate.ToString("yyyy-MM-dd", null);
            var publishedDateStr = ratesData.PublishedDate.ToString("yyyy-MM-dd", null);

            // CNB only provides foreign currency to CZK rates
            if (targetCurrency == CzechCurrencyCode)
            {
                var rate = FindRate(ratesData, sourceCurrency);
                if (rate != null)
                    return new ExchangeRateResponse
                    {
                        SourceCurrency = sourceCurrency,
                        TargetCurrency = CzechCurrencyCode,
                        Rate = rate.Rate / rate.Amount,
                        Date = dateStr,
                        DatePublished = publishedDateStr
                    };

                _logger.LogWarning("Rate not found for currency {SourceCurrency}/{TargetCurrency} on date {Date}",
                    sourceCurrency,
                    targetCurrency,
                    targetDate);
                throw new NotFoundException($"Rate not found for currency pair {sourceCurrency}/{targetCurrency} on date {targetDate}");
            }

            // For any other case (CZK as source or cross-currency), per requirements we don't calculate inverses
            _logger.LogWarning("Exchange rate not available for pair {SourceCurrency}/{TargetCurrency} as CNB only provides rates with CZK as target",
                sourceCurrency,
                targetCurrency);
            throw new NotFoundException(
                $"Exchange rate not available directly from CNB for pair {sourceCurrency}/{targetCurrency}. " +
                $"Only rates with CZK as the target currency (e.g., USD/CZK) are provided by CNB.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error getting exchange rate for pair {SourceCurrency}/{TargetCurrency} on date {Date}",
                sourceCurrency,
                targetCurrency,
                date);
            throw;
        }
    }

    public async Task<BatchExchangeRateResponse> GetBatchExchangeRatesAsync(
        BatchRateRequest request,
        CancellationToken cancellationToken = default)
    {
        LocalDate targetDate;
        if (string.IsNullOrWhiteSpace(request.Date))
        {
            targetDate = _dateTimeProvider.GetCurrentDate();
        }
        else
        {
            var parseResult = LocalDatePattern.Iso.Parse(request.Date);
            if (!parseResult.Success) throw new ArgumentException("Invalid date format. Date must be in ISO-8601 format (yyyy-MM-dd).", nameof(request.Date));

            targetDate = parseResult.Value;
        }

        // Get rates data to determine the published date
        var ratesData = await GetExchangeRatesAsync(targetDate, cancellationToken);
        var publishedDate = ratesData.PublishedDate;
        var dateStr = targetDate.ToString("yyyy-MM-dd", null);
        var publishedDateStr = publishedDate.ToString("yyyy-MM-dd", null);

        var rates = new List<ExchangeRateResponse>();

        foreach (var pairStr in request.CurrencyPairs)
            try
            {
                // Parse the currency pair
                var pair = CurrencyPairParser.ParseCurrencyPair(pairStr);

                // CNB only provides foreign currency to CZK rates
                // Only process pairs where the target currency is CZK
                if (pair.TargetCurrency == CzechCurrencyCode)
                {
                    var rate = FindRate(ratesData, pair.SourceCurrency);
                    if (rate != null)
                        rates.Add(new ExchangeRateResponse
                        {
                            SourceCurrency = pair.SourceCurrency,
                            TargetCurrency = CzechCurrencyCode,
                            Rate = rate.Rate / rate.Amount,
                            Date = dateStr,
                            DatePublished = publishedDateStr
                        });
                    else
                        _logger.LogWarning("Rate not found for currency {SourceCurrency}/{TargetCurrency} on date {Date}",
                            pair.SourceCurrency,
                            pair.TargetCurrency,
                            targetDate);
                }
                else
                {
                    _logger.LogWarning("Skipping pair {Pair} as CNB only provides rates with CZK as the target currency", pairStr);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to process currency pair {Pair} on date {Date}", pairStr, targetDate);
                // Skip this pair and continue with the next one
            }

        return new BatchExchangeRateResponse
        {
            Date = dateStr,
            DatePublished = publishedDateStr,
            Rates = rates
        };
    }

    private static CurrencyRate? FindRate(ExchangeRateData data,
        string currencyCode)
    {
        return data.Rates.FirstOrDefault(r => string.Equals(r.Currency, currencyCode, StringComparison.OrdinalIgnoreCase));
    }
}