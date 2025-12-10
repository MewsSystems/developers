using ExchangeRateUpdater.Features.ExchangeRates.Models;
using Microsoft.AspNetCore.WebUtilities;
using ZiggyCreatures.Caching.Fusion;

namespace ExchangeRateUpdater.Services;

public interface IAPIClient
{
    Task<List<ExchangeRateResponse>> GetExchangeRatesDailyAsync(DateOnly? date, string? language, CancellationToken ct);
}

public sealed class ExchangeRateResponseWrapper
{
    public List<ExchangeRateResponse> Rates { get; set; } = [];
}
public sealed class APIClient : IAPIClient
{
    private readonly HttpClient _httpClient;
    private readonly IFusionCache _cache;
    private readonly ILogger<APIClient> _logger;
    public APIClient(HttpClient httpClient, IFusionCache cache, ILogger<APIClient> logger)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<ExchangeRateResponse>> GetExchangeRatesDailyAsync(DateOnly? date, string? language, CancellationToken ct)
    {
        var queryParams = new Dictionary<string, string?>
        {
            {"date", DateTime.Now.ToString("yyyy-MM-dd")},
            {"lang", "CZ"}
        };

        if (date is not null)
        {
            queryParams["date"] = date.Value.ToString("yyyy-MM-dd");
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            queryParams["lang"] = language;
        }

        try
        {
            var cacheKey = $"exchangerates-date:{queryParams["date"]}-language:{queryParams["lang"]}";
            return await _cache.GetOrSetAsync(cacheKey,
                async _ =>
                {
                    string url = queryParams.Any() ? QueryHelpers.AddQueryString("daily", queryParams) : "daily";
                    var response = await _httpClient.GetAsync(url, ct);
                    response.EnsureSuccessStatusCode();

                    var wrapper = await response.Content.ReadFromJsonAsync<ExchangeRateResponseWrapper>(cancellationToken: ct);
                    return wrapper?.Rates ?? [];
                },
                options => options.SetDuration(TimeSpan.FromHours(1)),
                token: ct);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occured while fetching details from exchange api: {ex.Message}");
            throw;
        }
    }
}