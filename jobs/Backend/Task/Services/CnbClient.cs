using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services;

public sealed class CnbClient : ICnbClient
{
    private readonly HttpClient _http;
    private readonly ExchangeRateSettings _settings;
    private readonly ILogger<CnbClient> _logger;

    public CnbClient(HttpClient http, IOptions<ExchangeRateSettings> options, ILogger<CnbClient> logger)
    {
        _http = http;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<string> GetDailyRatesAsync(CancellationToken ct)
    {
        try
        {
            using var resp = await _http.GetAsync("", HttpCompletionOption.ResponseHeadersRead, ct);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync(ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            _logger.LogWarning("Fetching CNB daily rates was canceled.");
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error fetching CNB daily rates from {Url}", _settings.CnbDailyUrl);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch CNB daily rates from {Url}", _settings.CnbDailyUrl);
            throw;
        }
    }
}
