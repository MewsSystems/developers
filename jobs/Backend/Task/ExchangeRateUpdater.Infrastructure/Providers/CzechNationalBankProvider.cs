using System.Text.Json;
using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Extensions;
using ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.Providers;

/// <summary>
/// Exchange rate provider for Czech National Bank
/// </summary>
public class CzechNationalBankProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly CzechNationalBankOptions _options;
    private readonly ILogger<CzechNationalBankProvider> _logger;

    public string ProviderName => "Czech National Bank";
    public string BaseCurrency => "CZK";

    public CzechNationalBankProvider(
        HttpClient httpClient,
        IOptions<CzechNationalBankOptions> options,
        ILogger<CzechNationalBankProvider> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Maybe<IReadOnlyCollection<ExchangeRate>>> GetExchangeRatesForDate(Maybe<DateOnly> date)
    {
        var targetDate = date.GetValueOrDefault(DateHelper.Today);
        _logger.LogInformation($"Fetching exchange rates from {ProviderName} currencies for date {targetDate}");

        try
        {
            var url = BuildApiUrl(targetDate);
            _logger.LogInformation($"Requesting data from: {url}");

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var rates = ParseCnbJsonFormat(jsonContent);

            if (!rates.Any())
            {
                _logger.LogWarning("No rates found in API response");
                return Maybe<IReadOnlyCollection<ExchangeRate>>.Nothing;
            }

            _logger.LogInformation($"Successfully retrieved {rates.Count} exchange rates from {ProviderName}");

            return rates.AsReadOnlyCollection().AsMaybe();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"HTTP error occurred while fetching exchange rates from {ProviderName}");
            throw new ExchangeRateProviderException($"Failed to fetch exchange rates from {ProviderName}", ex);
        }
        catch (FormatException ex)
        {
            _logger.LogError(ex, $"Parsing error occurred while processing response from {ProviderName}");
            throw new ExchangeRateProviderException($"Failed to parse response from {ProviderName}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unexpected error occurred while fetching exchange rates from {ProviderName}");
            throw new ExchangeRateProviderException($"Unexpected error occurred while fetching exchange rates from {ProviderName}", ex);
        }
    }

    private string BuildApiUrl(DateOnly date)
    {
        var dateString = date.ToString(_options.DateFormat);
        return $"{_options.BaseUrl}?date={dateString}&lang={_options.Language}";
    }

    private List<ExchangeRate> ParseCnbJsonFormat(string jsonContent)
    {
        var rates = new List<ExchangeRate>();

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var apiResponse = JsonSerializer.Deserialize<CnbApiResponse>(jsonContent, options);

            if (apiResponse?.Rates == null)
            {
                _logger.LogWarning("No rates found in JSON response");
                return rates;
            }

            foreach (var rateDto in apiResponse.Rates)
            {
                // CNB rates are given as: amount units = rate CZK
                // We want: 1 unit = (rate / amount) CZK
                var ratePerUnit = rateDto.Rate / rateDto.Amount;
                var sourceCurrency = new Currency(rateDto.CurrencyCode);
                var targetCurrency = new Currency(BaseCurrency);

                DateOnly exchangeDate = DateOnly.TryParse(rateDto.ValidFor, out var parsedDate) ? parsedDate : DateHelper.Today;

                var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, ratePerUnit, exchangeDate);
                rates.Add(exchangeRate);
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error parsing CNB JSON format");
            throw new ExchangeRateProviderException("Failed to parse CNB JSON response format", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing CNB JSON response");
            throw new ExchangeRateProviderException("Failed to process CNB JSON response", ex);
        }

        return rates;
    }
}
