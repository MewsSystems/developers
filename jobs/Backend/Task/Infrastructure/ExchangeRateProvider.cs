using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure;

public sealed class ExchangeRateProvider : IExchangeRateProvider, IDisposable
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly CnbOptions _options;
    private readonly ILogger<ExchangeRateProvider> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly SemaphoreSlim _refreshSemaphore = new(1, 1);
    private CacheSnapshot? _snapshot;

    public ExchangeRateProvider(
        IHttpClientFactory httpClientFactory,
        IOptions<CnbOptions> options,
        ILogger<ExchangeRateProvider> logger,
        TimeProvider? timeProvider = null)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        ArgumentNullException.ThrowIfNull(currencies);

        HashSet<string> requestedCodes = NormalizeRequestedCodes(currencies);
        if (requestedCodes.Count == 0)
        {
            return [];
        }

        CacheSnapshot? snapshot = Volatile.Read(ref _snapshot);
        if (snapshot is not null && IsFresh(snapshot))
        {
            _logger.LogDebug("Serving exchange rates from cache (cached at {CachedAt}).", snapshot.CachedAt);
            return FilterRates(snapshot.Rates, requestedCodes);
        }

        _refreshSemaphore.Wait();
        try
        {
            snapshot = Volatile.Read(ref _snapshot);
            if (snapshot is not null && IsFresh(snapshot))
            {
                _logger.LogDebug("Serving exchange rates from cache (cached at {CachedAt}).", snapshot.CachedAt);
                return FilterRates(snapshot.Rates, requestedCodes);
            }

            CacheSnapshot refreshedSnapshot = FetchSnapshot(snapshot);
            Volatile.Write(ref _snapshot, refreshedSnapshot);
            return FilterRates(refreshedSnapshot.Rates, requestedCodes);
        }
        finally
        {
            _refreshSemaphore.Release();
        }
    }

    private CacheSnapshot FetchSnapshot(CacheSnapshot? currentSnapshot)
    {
        HttpClient client = _httpClientFactory.CreateClient("cnb");
        using HttpResponseMessage response = SendRequest(client, ifModifiedSince: currentSnapshot?.LastModified);

        if (response.StatusCode == HttpStatusCode.NotModified)
        {
            if (currentSnapshot is not null)
            {
                _logger.LogDebug("CNB returned 304 Not Modified. Reusing cached snapshot.");
                return currentSnapshot with { CachedAt = _timeProvider.GetUtcNow() };
            }

            _logger.LogWarning("CNB returned 304 without an existing snapshot. Retrying with a full GET.");
            using HttpResponseMessage fallbackResponse = SendRequest(client, ifModifiedSince: null);
            return BuildSnapshotFromSuccessResponse(fallbackResponse);
        }

        return BuildSnapshotFromSuccessResponse(response);
    }

    private CacheSnapshot BuildSnapshotFromSuccessResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"CNB request failed with status code {(int)response.StatusCode} ({response.StatusCode}).",
                inner: null,
                response.StatusCode);
        }

        using Stream stream = response.Content.ReadAsStream();
        using StreamReader reader = new(stream, leaveOpen: true);
        string rawContent = reader.ReadToEnd();

        ExchangeRate[] rates = CnbDailyTxtParser.Parse(rawContent).ToArray();
        DateTimeOffset now = _timeProvider.GetUtcNow();
        DateTimeOffset? lastModified = response.Content.Headers.LastModified;

        _logger.LogDebug("Fetched {RatesCount} rates from CNB.", rates.Length);
        return new CacheSnapshot(rates, now, lastModified);
    }

    private HttpResponseMessage SendRequest(HttpClient client, DateTimeOffset? ifModifiedSince)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, _options.Url);
        if (ifModifiedSince is not null)
        {
            request.Headers.IfModifiedSince = ifModifiedSince;
        }

        return client.Send(request, HttpCompletionOption.ResponseHeadersRead);
    }

    private HashSet<string> NormalizeRequestedCodes(IEnumerable<Currency> currencies)
    {
        HashSet<string> requestedCodes = new(StringComparer.Ordinal);
        foreach (Currency currency in currencies)
        {
            if (currency is null)
            {
                throw new ArgumentException("Currencies collection must not contain null elements.", nameof(currencies));
            }

            if (currency.Code == "CZK")
            {
                continue;
            }

            requestedCodes.Add(currency.Code);
        }

        return requestedCodes;
    }

    private bool IsFresh(CacheSnapshot snapshot)
    {
        TimeSpan ttl = TimeSpan.FromSeconds(_options.CacheTtlSeconds);
        DateTimeOffset now = _timeProvider.GetUtcNow();
        return now - snapshot.CachedAt < ttl;
    }

    private static ExchangeRate[] FilterRates(IEnumerable<ExchangeRate> rates, HashSet<string> requestedCodes)
    {
        return rates.Where(rate => requestedCodes.Contains(rate.SourceCurrency.Code)).ToArray();
    }

    public void Dispose() => _refreshSemaphore.Dispose();
}
