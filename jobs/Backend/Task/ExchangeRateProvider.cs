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

namespace ExchangeRateUpdater;

public sealed class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _http;
    private readonly IDistributedCache _cache;
    private readonly ILogger<ExchangeRateProvider> _log;
    private readonly CnbOptions _opt;
    private readonly IAsyncPolicy<HttpResponseMessage> _policy;

    public ExchangeRateProvider(
        HttpClient httpClient,
        IDistributedCache cache,
        IOptions<CnbOptions> options,
        ILogger<ExchangeRateProvider> log)
    {
        _http = httpClient;
        _cache = cache;
        _log = log;
        _opt = options.Value;

        _http.Timeout = _opt.HttpTimeout;
        _http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mews-BackendTask", "1.0"));
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

        var cached = await CacheGetAsync(key, ct);
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
            var resp = await _policy.ExecuteAsync((_, token) => _http.GetAsync(url, token), ctx, ct);
            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"CNB JSON request failed: {(int)resp.StatusCode} {resp.ReasonPhrase}");

            var payload = await resp.Content.ReadAsStringAsync(ct);
            var rows = ParseJsonRows(payload, resolved);

            var normalized = rows.Select(r =>
                new ExchangeRate(r.CurrencyCode.ToString(), "CZK", r.RateCzk / r.Amount, r.ValidFor)
            ).ToList();

            await CacheSetAsync(key, normalized, ct);
            _log.LogInformation("Cached {Count} rates for {Date}", normalized.Count, normalized.First().ValidFor.ToString("yyyy-MM-dd"));
            return normalized;
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "JSON API fetch failed; attempting stale cache for {ResolvedDate}", resolved.ToString("yyyy-MM-dd"));

            foreach (var prev in PreviousBusinessDays(resolved, 7))
            {
                var prevKey = $"{_opt.CacheKeyPrefix}{prev:yyyy-MM-dd}";
                var stale = await CacheGetAsync(prevKey, ct);
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

    private static List<(string CurrencyCode, int Amount, decimal RateCzk, DateOnly ValidFor)> ParseJsonRows(string json, DateOnly fallbackDate)
    {
        using var doc = JsonDocument.Parse(json);
        IEnumerable<JsonElement> items = doc.RootElement.ValueKind switch
        {
            JsonValueKind.Array => doc.RootElement.EnumerateArray(),
            JsonValueKind.Object when doc.RootElement.TryGetProperty("items", out var itemsProp) => itemsProp.EnumerateArray(),
            JsonValueKind.Object when doc.RootElement.TryGetProperty("rates", out var ratesProp) => ratesProp.EnumerateArray(),
            _ => Array.Empty<JsonElement>()
        };

        var list = new List<(string, int, decimal, DateOnly)>(40);
        foreach (var el in items)
        {
            if (!TryString(el, "CurrencyCode", out string? code)) continue;
            if (!TryInt(el, "Amount", out int amount) || amount <= 0) continue;
            if (!TryDecimal(el, "Rate", out decimal rate) || rate <= 0) continue;

            var validFor = fallbackDate;
            if (TryDateTime(el, "ValidFor", out DateTime dt)) validFor = DateOnly.FromDateTime(dt);

            list.Add((code!.ToUpperInvariant(), amount, rate, validFor));
        }

        if (list.Count == 0)
            throw new FormatException("CNB JSON payload did not contain usable rates.");

        return list;

        static bool TryString(JsonElement e, string n, out string? v)
        {
            v = null;
            foreach (var p in e.EnumerateObject())
                if (string.Equals(p.Name, n, StringComparison.OrdinalIgnoreCase))
                { v = p.Value.GetString(); return v is not null; }
            return false;
        }

        static bool TryInt(JsonElement e, string n, out int v)
        {
            v = default;
            foreach (var p in e.EnumerateObject())
                if (string.Equals(p.Name, n, StringComparison.OrdinalIgnoreCase))
                {
                    if (p.Value.ValueKind == JsonValueKind.Number && p.Value.TryGetInt32(out var i)) { v = i; return true; }
                    if (p.Value.ValueKind == JsonValueKind.String && int.TryParse(p.Value.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var j)) { v = j; return true; }
                }
            return false;
        }

        static bool TryDecimal(JsonElement e, string n, out decimal v)
        {
            v = default;
            foreach (var p in e.EnumerateObject())
                if (string.Equals(p.Name, n, StringComparison.OrdinalIgnoreCase))
                {
                    if (p.Value.ValueKind == JsonValueKind.Number && p.Value.TryGetDecimal(out var d)) { v = d; return true; }
                    if (p.Value.ValueKind == JsonValueKind.String && decimal.TryParse(p.Value.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d)) { v = d; return true; }
                }
            return false;
        }

        static bool TryDateTime(JsonElement e, string n, out DateTime v)
        {
            v = default;
            foreach (var p in e.EnumerateObject())
                if (string.Equals(p.Name, n, StringComparison.OrdinalIgnoreCase) &&
                    p.Value.ValueKind == JsonValueKind.String &&
                    DateTime.TryParse(p.Value.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dt))
                { v = dt; return true; }
            return false;
        }
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

    private async Task<List<ExchangeRate>?> CacheGetAsync(string key, CancellationToken ct)
    {
        try
        {
            var hit = await _cache.GetStringAsync(key, ct);
            return string.IsNullOrEmpty(hit) ? null : JsonSerializer.Deserialize<List<ExchangeRate>>(hit)!;
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Redis get failed (key: {Key})", key);
            return null;
        }
    }

    private async Task CacheSetAsync(string key, IReadOnlyList<ExchangeRate> value, CancellationToken ct)
    {
        try
        {
            var payload = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, payload, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _opt.CacheTtl
            }, ct);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Redis set failed (key: {Key})", key);
        }
    }
}