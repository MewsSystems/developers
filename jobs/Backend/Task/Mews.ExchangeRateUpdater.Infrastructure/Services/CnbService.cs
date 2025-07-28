using System.Text.Json;
using Mews.ExchangeRateUpdater.Domain.Interfaces;
using Mews.ExchangeRateUpdater.Domain.ValueObjects;
using Mews.ExchangeRateUpdater.Infrastructure.Dtos;
using Mews.ExchangeRateUpdater.Infrastructure.Exceptions;
using Mews.ExchangeRateUpdater.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Mews.ExchangeRateUpdater.Infrastructure.Services;

public class CnbService : ICnbService
{
    private const string ExratesEndpoint = "exrates/daily?lang=EN";

    private readonly HttpClient _httpClient;
    private readonly ICnbParser _parser;
    private readonly ILogger<CnbService> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    public CnbService(HttpClient httpClient, ICnbParser parser, ILogger<CnbService> logger)
    {
        _httpClient = httpClient;
        _parser = parser;
        _logger = logger;
    }

    public async Task<IEnumerable<ExchangeRate>> GetLatestRatesAsync(CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Fetching latest exchange rates from CNB API at {Url}", ExratesEndpoint);

            var response = await _httpClient.GetAsync(ExratesEndpoint, ct);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(ct);
            var result = await JsonSerializer.DeserializeAsync<CnbResponse>(stream, JsonOptions, ct);

            var rates = _parser.Parse(result);
            _logger.LogInformation("Parsed {Count} exchange rates", rates.Count());

            return rates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch or parse CNB JSON rates");
            throw new CnbServiceException("Error fetching/parsing CNB rates from JSON API.", ex);
        }
    }
}