using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Common;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Data;

public class CnbExchangeRateRepository : ICnbExchangeRateRepository
{
    private readonly ICnbApiClient _apiClient;
    private readonly ILogger<CnbExchangeRateRepository> _logger;
    private readonly Currency _baseCurrency = new Currency("CZK");

    /// <summary>
    /// Initializes a new instance of the <see cref="CnbExchangeRateRepository"/> class.
    /// </summary>
    /// <param name="apiClient"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CnbExchangeRateRepository(ICnbApiClient apiClient, ILogger<CnbExchangeRateRepository> logger)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Asynchronously retrieves the latest exchange rates from the Czech National Bank (CNB) API.
    /// </summary>
    /// <returns>A collection of <see cref="ExchangeRate"/> objects representing the latest exchange rates.</returns>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving exchange rates from CNB API.");
            var response = await _apiClient.GetLatestExchangeRatesAsync();
            var rates = ParseExchangeRates(response);
            _logger.LogInformation($"Successfully retrieved {rates.Count()} exchange rates.");
            return rates;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve exchange rates from CNB API.");
            throw new HttpRequestException("Could not retrieve exchange rates from CNB API.", e);
        }
    }

    /// <summary>
    /// Asynchronously retrieves specific exchange rates for the given currencies.
    /// </summary>
    /// <param name="currencies"></param>
    /// <returns>A collection of <see cref="ExchangeRate"/> objects for the specified currencies.</returns>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<ExchangeRate>> GetSpecificExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        try
        {
            _logger.LogInformation("Retrieving specific exchange rates from CNB API for currencies: {Currencies}", string.Join(", ", currencies));
            var allRates = await GetExchangeRatesAsync();
            var specificRates = allRates.Where(rate => currencies.Contains(rate.SourceCurrency));
            _logger.LogInformation("Successfully retrieved specific exchange rates.");
            return specificRates;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve specific exchange rates from CNB API.");
            throw new HttpRequestException("Could not retrieve specific exchange rates from CNB API.", e);
        }
    }

    private IEnumerable<ExchangeRate> ParseExchangeRates(string responseText)
    {
        if (string.IsNullOrWhiteSpace(responseText))
        {
            throw new ArgumentException("Exchange rate response text cannot be null or empty.", nameof(responseText));
        }

        string[] lines = responseText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        char delimiter = TextParsingUtils.DetectDelimiter(lines);
        int headerRowIndex = TextParsingUtils.FindHeaderRowIndex(lines, delimiter);

        if (headerRowIndex == -1)
        {
            _logger.LogError("Could not find header row in exchange rate response.");
            yield break;
        }

        foreach (var line in lines.Skip(headerRowIndex + 1))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(delimiter);
            if (parts.Length < 5)
            {
                _logger.LogWarning("Skipping invalid exchange rate line format: {Line}", line);
                continue;
            }

            if (!int.TryParse(parts[2], out var amount))
            {
                _logger.LogWarning("Skipping line due to invalid amount format: {Line}", line);
                continue;
            }

            if (!decimal.TryParse(parts[4], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var rateValue))
            {
                _logger.LogWarning("Skipping line due to invalid rate value format: {Line}", line);
                continue;
            }

            var normalizedRate = rateValue / amount;

            var currency = new Currency(parts[3]);
            yield return new ExchangeRate(
                sourceCurrency: currency,
                targetCurrency: _baseCurrency,
                value: normalizedRate);
        }
    }
}