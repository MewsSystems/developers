using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Src.Cnb;
using ExchangeRateUpdater.Src.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Src;

public sealed class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _http;
    private readonly ILogger<ExchangeRateProvider> _log;
    private readonly CnbOptions _opt;
    private readonly IAsyncPolicy<HttpResponseMessage> _policy;
    private readonly IExchangeRateCache _rateCache;

    // ctor
    public ExchangeRateProvider(
        HttpClient httpClient,
        IExchangeRateCache rateCache,
        IOptions<CnbOptions> options,
        ILogger<ExchangeRateProvider> log)
    {
        _http = httpClient;
        _rateCache = rateCache;
        _log = log;
        _opt = options.Value;

        _http.Timeout = _opt.HttpTimeout;
        _http.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Mews-BackendTask", "1.0"));
        _policy = Policies.CreateHttpPolicy(_opt.RetryCount, _log);
    }

    public async Task<List<ExchangeRate>> GetAsync(DateOnly date, CancellationToken ct = default)
    {
        using var _ = _log.BeginScope(new Dictionary<string, object?>
        {
            ["requested_date"] = date.ToString("yyyy-MM-dd"),
            ["source"] = "json-api"
        });

        var resolved = ResolveBusinessDate(date);
        _log.LogInformation("Fetching CNB JSON API (resolved_date: {ResolvedDate})", resolved.ToString("yyyy-MM-dd"));

        var key = $"{_opt.CacheKeyPrefix}{resolved:yyyy-MM-dd}";

        var cached = await _rateCache.GetAsync(key, ct);
        if (cached is not null)
        {
            _log.LogInformation("Cache hit for {ResolvedDate}", resolved.ToString("yyyy-MM-dd"));
            return cached;
        }
        _log.LogInformation("Cache miss for {ResolvedDate}", resolved.ToString("yyyy-MM-dd"));

        string url = BuildJsonUrl(resolved);
        var ctx = new Context();
        ctx["url"] = url;
        ctx["date"] = resolved.ToString("yyyy-MM-dd");

        try
        {
            HttpResponseMessage resp = await _policy.ExecuteAsync((_, token) => _http.GetAsync(url, token), ctx, ct);
            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"CNB JSON request failed: {(int)resp.StatusCode} {resp.ReasonPhrase}");

            string payload = await resp.Content.ReadAsStringAsync(ct);
            List<ExchangeRate> rates = ParseJsonRows(payload, resolved);

            await _rateCache.SetAsync(key, rates, ct);
            _log.LogInformation("Cached {Count} rates for {Date}", rates.Count, rates[0].ValidFor.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            return rates;
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "JSON API fetch failed; attempting stale cache for {ResolvedDate}", resolved.ToString("yyyy-MM-dd"));

            foreach (var prev in PreviousBusinessDays(resolved, 7))
            {
                var prevKey = $"{_opt.CacheKeyPrefix}{prev:yyyy-MM-dd}";
                var stale = await _rateCache.GetAsync(prevKey, ct);
                if (stale is not null)
                {
                    _log.LogInformation("Serving stale data for {StaleDate}", prev.ToString("yyyy-MM-dd"));
                    return stale;
                }
            }

            _log.LogError(ex, "No live data and no stale cache available for {ResolvedDate}", resolved.ToString("yyyy-MM-dd"));
            throw;
        }
    }

    private string BuildJsonUrl(DateOnly date)
    {
        var baseUrl = _opt.JsonApiBase.TrimEnd('/');
        var path = _opt.JsonDailyEndpoint.StartsWith('/') ? _opt.JsonDailyEndpoint : "/" + _opt.JsonDailyEndpoint;
        var d = date.ToDateTime(TimeOnly.MinValue).ToString(_opt.JsonDateFormat, CultureInfo.InvariantCulture);
        return $"{baseUrl}{path}?{_opt.JsonDateParam}={Uri.EscapeDataString(d)}";
    }

    private static List<ExchangeRate> ParseJsonRows(string json, DateOnly fallbackDate)
    {
        JsonSerializerOptions opts = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        CnbRate[]? arr = JsonSerializer.Deserialize<CnbRate[]>(json, opts);
        IEnumerable<CnbRate>? source = (arr is { Length: > 0 }) ? arr : null;

        if (source is null)
        {
            CnbDaily? daily = JsonSerializer.Deserialize<CnbDaily>(json, opts);
            source = daily?.Items;
        }

        if (source is null)
            throw new FormatException("CNB JSON payload did not contain usable rates.");

        List<ExchangeRate> list = new List<ExchangeRate>();
        foreach (CnbRate r in source)
        {
            if (string.IsNullOrWhiteSpace(r.CurrencyCode) || r.Amount <= 0 || r.Rate <= 0m)
                continue;

            DateOnly validFor = r.ValidFor.HasValue
                ? DateOnly.FromDateTime(r.ValidFor.Value)
                : fallbackDate;

            decimal perUnit = r.Rate / r.Amount;
            list.Add(new ExchangeRate(r.CurrencyCode.ToUpperInvariant(), "CZK", perUnit, validFor));
        }

        if (list.Count == 0)
            throw new FormatException("CNB JSON payload did not contain usable rates.");

        return list;
    }

    private sealed class CnbDaily
    {
        [JsonPropertyName("items")]
        public List<CnbRate>? Items { get; set; }
    }

    private sealed class CnbRate
    {
        public string? CurrencyCode { get; set; }
        public int Amount { get; set; }
        public decimal Rate { get; set; }
        public DateTime? ValidFor { get; set; }
    }

    private DateOnly ResolveBusinessDate(DateOnly requested)
    {
        var d = requested;
        if (d.DayOfWeek is DayOfWeek.Saturday) d = d.AddDays(-1);
        else if (d.DayOfWeek is DayOfWeek.Sunday) d = d.AddDays(-2);

        if (!_opt.EnablePublishTimeFallback) return d;

        try
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(_opt.PublishTimeZone);
            var nowTz = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz);
            var today = DateOnly.FromDateTime(nowTz.Date);
            if (d == today)
            {
                DateTime publishAt = nowTz.Date + _opt.PublishTime.ToTimeSpan();
                if (nowTz.DateTime < publishAt)
                {
                    d = PreviousBusinessDay(d);
                    _log.LogInformation("Before publish time; using previous business day {ResolvedDate}", d.ToString("yyyy-MM-dd"));
                }
            }
        }
        catch (TimeZoneNotFoundException)
        {
            _log.LogWarning("Timezone {TimeZone} not found; skipping publish fallback.", _opt.PublishTimeZone);
        }
        return d;
    }

    private static DateOnly PreviousBusinessDay(DateOnly date)
    {
        var d = date.AddDays(-1);
        if (d.DayOfWeek == DayOfWeek.Sunday) d = d.AddDays(-2);
        else if (d.DayOfWeek == DayOfWeek.Saturday) d = d.AddDays(-1);
        return d;
    }

    private static IEnumerable<DateOnly> PreviousBusinessDays(DateOnly start, int days)
    {
        var d = start;
        for (int i = 0; i < days; i++) { d = PreviousBusinessDay(d); yield return d; }
    }
}