using System.Globalization;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NodaTime;

namespace ExchangeRateUpdater.Infrastructure.DataSources.Cnb;

/// <summary>
/// Data source for the Czech National Bank (CNB) exchange rates.
/// Retrieves and parses exchange rate data from the CNB API.
/// 
/// API format example:
/// https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=16.05.2025
///
/// Response format:
/// 16 May 2025 #93
/// Country|Currency|Amount|Code|Rate
/// Australia|dollar|1|AUD|14.280
/// ...
/// </summary>
public class CnbExchangeRateDataSource : IExchangeRateDataSource
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CnbExchangeRateDataSource> _logger;
    private readonly ExchangeRateServiceOptions _options;
    private const string CzechCurrencyCode = "CZK";

    public CnbExchangeRateDataSource(
        HttpClient httpClient,
        ILogger<CnbExchangeRateDataSource> logger,
        IOptions<ExchangeRateServiceOptions> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<ExchangeRateData> GetExchangeRatesAsync(LocalDate date,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = GetCnbApiUrl(date);
            _logger.LogInformation("Fetching exchange rates from CNB URL: {Url}", url);

            var content = await _httpClient.GetStringAsync(url, cancellationToken);

            return ParseCnbData(content, date);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching exchange rates from CNB for date {Date}", date);
            throw;
        }
    }

    public async Task<bool> IsDataAvailableForDateAsync(LocalDate date,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = GetCnbApiUrl(date);
            _logger.LogInformation("Checking data availability from CNB URL: {Url}", url);

            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking data availability from CNB for date {Date}", date);
            return false;
        }
    }

    private string GetCnbApiUrl(LocalDate date)
    {
        // CNB API uses dd.MM.yyyy format for the date parameter
        var dateString = date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        return $"{_options.CnbApiBaseUrl.TrimEnd('/')}/daily.txt?date={dateString}";
    }

    private ExchangeRateData ParseCnbData(string content,
        LocalDate date)
    {
        var rates = new List<CurrencyRate>();
        var lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // CNB format example:
        // First line: "16 May 2025 #93"
        // Second line: "Country|Currency|Amount|Code|Rate"
        // Data lines: "USA|dollar|1|USD|22.274"
        if (lines.Length < 3)
        {
            _logger.LogWarning("Invalid CNB data format: less than 3 lines for date {Date}", date);
            return new ExchangeRateData { Date = date, PublishedDate = date, Rates = rates };
        }

        _logger.LogDebug("CNB data header: {Header}", lines[0]);
        _logger.LogDebug("CNB data columns: {Columns}", lines[1]);

        for (var i = 2; i < lines.Length; i++) // Skip header lines
        {
            var parts = lines[i].
                Split('|');
            if (parts.Length < 5)
            {
                _logger.LogWarning("Skipping invalid line format: {Line}", lines[i]);
                continue;
            }

            try
            {
                var country = parts[0].
                    Trim();
                var currencyName = parts[1].
                    Trim();
                var amount = decimal.Parse(parts[2].
                        Trim(),
                    CultureInfo.InvariantCulture);
                var currencyCode = parts[3].
                    Trim();
                var rate = decimal.Parse(parts[4].
                        Trim().
                        Replace(",", "."),
                    CultureInfo.InvariantCulture);

                rates.Add(new CurrencyRate
                {
                    Currency = currencyCode,
                    Country = country,
                    Amount = amount,
                    Rate = rate
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error parsing currency line: {Line}", lines[i]);
            }
        }

        _logger.LogInformation("Successfully parsed {Count} currency rates for date {Date}", rates.Count, date);

        return new ExchangeRateData
        {
            Date = date,
            PublishedDate = date,
            Rates = rates
        };
    }
}