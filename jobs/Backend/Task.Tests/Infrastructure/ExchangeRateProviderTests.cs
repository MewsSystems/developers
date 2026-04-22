using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace Task.Tests.Infrastructure;

public sealed class ExchangeRateProviderTests
{
    [Fact]
    public void GetExchangeRates_null_currencies_throws()
    {
        ExchangeRateProvider provider = CreateProvider(_ => CreateOkResponse(DefaultPayload));

        Assert.Throws<ArgumentNullException>(() => provider.GetExchangeRates(null!));
    }

    [Fact]
    public void GetExchangeRates_empty_currencies_returns_empty()
    {
        var factory = new FakeHttpClientFactory(new HttpClient(new DelegateHttpMessageHandler(_ => CreateOkResponse(DefaultPayload))));
        ExchangeRateProvider provider = CreateProvider(factory);

        ExchangeRate[] rates = provider.GetExchangeRates([]).ToArray();

        Assert.Empty(rates);
        Assert.Equal(0, factory.CreateClientCalls);
    }

    [Fact]
    public void GetExchangeRates_filters_only_requested_codes()
    {
        ExchangeRateProvider provider = CreateProvider(_ => CreateOkResponse(DefaultPayload));

        ExchangeRate[] rates = provider.GetExchangeRates([new Currency("USD"), new Currency("EUR")]).ToArray();

        Assert.Equal(2, rates.Length);
        Assert.Contains(rates, rate => rate.SourceCurrency.Code == "USD");
        Assert.Contains(rates, rate => rate.SourceCurrency.Code == "EUR");
        Assert.DoesNotContain(rates, rate => rate.SourceCurrency.Code == "AUD");
    }

    [Fact]
    public void GetExchangeRates_ignores_unknown_currency()
    {
        ExchangeRateProvider provider = CreateProvider(_ => CreateOkResponse(DefaultPayload));

        ExchangeRate[] rates = provider.GetExchangeRates([new Currency("XYZ")]).ToArray();

        Assert.Empty(rates);
    }

    [Fact]
    public void GetExchangeRates_case_insensitive_currency_codes()
    {
        ExchangeRateProvider provider = CreateProvider(_ => CreateOkResponse(DefaultPayload));

        ExchangeRate[] rates = provider.GetExchangeRates([new Currency("usd"), new Currency("eUr")]).ToArray();

        Assert.Equal(2, rates.Length);
        Assert.Contains(rates, rate => rate.SourceCurrency.Code == "USD");
        Assert.Contains(rates, rate => rate.SourceCurrency.Code == "EUR");
    }

    [Fact]
    public void GetExchangeRates_without_czk_in_request_still_returns_rates()
    {
        ExchangeRateProvider provider = CreateProvider(_ => CreateOkResponse(DefaultPayload));

        ExchangeRate[] rates = provider.GetExchangeRates([new Currency("USD")]).ToArray();

        ExchangeRate rate = Assert.Single(rates);
        Assert.Equal("USD", rate.SourceCurrency.Code);
        Assert.Equal("CZK", rate.TargetCurrency.Code);
        Assert.Equal(22.640m, rate.Value);
    }

    [Fact]
    public void GetExchangeRates_czk_in_request_is_silently_ignored()
    {
        ExchangeRateProvider provider = CreateProvider(_ => CreateOkResponse(DefaultPayload));

        ExchangeRate[] rates = provider.GetExchangeRates([new Currency("CZK"), new Currency("USD")]).ToArray();

        ExchangeRate rate = Assert.Single(rates);
        Assert.Equal("USD", rate.SourceCurrency.Code);
    }

    [Fact]
    public void GetExchangeRates_only_czk_returns_empty_without_calling_http()
    {
        var factory = new FakeHttpClientFactory(new HttpClient(new DelegateHttpMessageHandler(_ => CreateOkResponse(DefaultPayload))));
        ExchangeRateProvider provider = CreateProvider(factory);

        ExchangeRate[] rates = provider.GetExchangeRates([new Currency("CZK"), new Currency(" czk ")]).ToArray();

        Assert.Empty(rates);
        Assert.Equal(0, factory.CreateClientCalls);
    }

    [Fact]
    public void GetExchangeRates_http_error_throws()
    {
        ExchangeRateProvider provider = CreateProvider(_ => new HttpResponseMessage(HttpStatusCode.BadGateway));

        HttpRequestException ex = Assert.Throws<HttpRequestException>(
            () => provider.GetExchangeRates([new Currency("USD")]).ToArray());

        Assert.Equal(HttpStatusCode.BadGateway, ex.StatusCode);
    }

    [Fact]
    public void GetExchangeRates_304_with_cache_returns_cached()
    {
        DateTimeOffset expectedLastModified = new(2026, 04, 22, 10, 0, 0, TimeSpan.Zero);
        int requestCount = 0;
        List<DateTimeOffset?> ifModifiedSinceValues = [];
        ExchangeRateProvider provider = CreateProvider(
            request =>
            {
                requestCount++;
                ifModifiedSinceValues.Add(request.Headers.IfModifiedSince);
                return requestCount switch
                {
                    1 => CreateOkResponse(DefaultPayload, expectedLastModified),
                    2 => CreateNotModifiedResponse(),
                    _ => throw new InvalidOperationException("Unexpected request count.")
                };
            },
            cacheTtlSeconds: 0);

        _ = provider.GetExchangeRates([new Currency("USD")]).ToArray();
        ExchangeRate[] refreshedRates = provider.GetExchangeRates([new Currency("EUR")]).ToArray();

        ExchangeRate rate = Assert.Single(refreshedRates);
        Assert.Equal("EUR", rate.SourceCurrency.Code);
        Assert.Equal(2, requestCount);
        Assert.Null(ifModifiedSinceValues[0]);
        Assert.Equal(expectedLastModified, ifModifiedSinceValues[1]);
    }

    [Fact]
    public void GetExchangeRates_304_without_cache_fallback_to_full_get()
    {
        int requestCount = 0;
        List<DateTimeOffset?> ifModifiedSinceValues = [];
        ExchangeRateProvider provider = CreateProvider(
            request =>
            {
                requestCount++;
                ifModifiedSinceValues.Add(request.Headers.IfModifiedSince);
                return requestCount switch
                {
                    1 => CreateNotModifiedResponse(),
                    2 => CreateOkResponse(DefaultPayload),
                    _ => throw new InvalidOperationException("Unexpected request count.")
                };
            },
            cacheTtlSeconds: 0);

        ExchangeRate[] rates = provider.GetExchangeRates([new Currency("USD")]).ToArray();

        ExchangeRate rate = Assert.Single(rates);
        Assert.Equal("USD", rate.SourceCurrency.Code);
        Assert.Equal(2, requestCount);
        Assert.Null(ifModifiedSinceValues[0]);
        Assert.Null(ifModifiedSinceValues[1]);
    }

    [Fact]
    public async global::System.Threading.Tasks.Task GetExchangeRates_concurrent_calls_only_one_http_request()
    {
        int requestCount = 0;
        ExchangeRateProvider provider = CreateProvider(
            _ =>
            {
                Interlocked.Increment(ref requestCount);
                Thread.Sleep(100);
                return CreateOkResponse(DefaultPayload);
            });

        global::System.Threading.Tasks.Task<ExchangeRate[]> task1 =
            global::System.Threading.Tasks.Task.Run(() => provider.GetExchangeRates([new Currency("USD")]).ToArray());
        global::System.Threading.Tasks.Task<ExchangeRate[]> task2 =
            global::System.Threading.Tasks.Task.Run(() => provider.GetExchangeRates([new Currency("EUR")]).ToArray());

        await global::System.Threading.Tasks.Task.WhenAll(task1, task2);

        Assert.Equal(1, requestCount);
    }

    [Fact]
    public void GetExchangeRates_returns_defensive_copy_not_internal_reference()
    {
        ExchangeRateProvider provider = CreateProvider(_ => CreateOkResponse(DefaultPayload));

        ExchangeRate[] firstResult = provider.GetExchangeRates([new Currency("USD")]).ToArray();
        firstResult[0] = new ExchangeRate(new Currency("BROKEN"), new Currency("CZK"), 0.01m);

        ExchangeRate[] secondResult = provider.GetExchangeRates([new Currency("USD")]).ToArray();

        ExchangeRate rate = Assert.Single(secondResult);
        Assert.Equal("USD", rate.SourceCurrency.Code);
    }

    [Fact]
    public void GetExchangeRates_trims_currency_codes()
    {
        ExchangeRateProvider provider = CreateProvider(_ => CreateOkResponse(DefaultPayload));

        ExchangeRate[] rates = provider.GetExchangeRates([new Currency(" usd "), new Currency(" eur\t")]).ToArray();

        Assert.Equal(2, rates.Length);
        Assert.Contains(rates, rate => rate.SourceCurrency.Code == "USD");
        Assert.Contains(rates, rate => rate.SourceCurrency.Code == "EUR");
    }

    [Fact]
    public void GetExchangeRates_duplicate_requested_codes_are_deduplicated()
    {
        ExchangeRateProvider provider = CreateProvider(_ => CreateOkResponse(DefaultPayload));

        ExchangeRate[] rates = provider.GetExchangeRates(
            [new Currency("USD"), new Currency(" usd "), new Currency("USD")]).ToArray();

        ExchangeRate rate = Assert.Single(rates);
        Assert.Equal("USD", rate.SourceCurrency.Code);
    }

    [Fact]
    public void GetExchangeRates_within_ttl_does_not_call_http()
    {
        FakeTimeProvider timeProvider = new(new DateTimeOffset(2026, 04, 22, 10, 0, 0, TimeSpan.Zero));
        int requestCount = 0;
        ExchangeRateProvider provider = CreateProvider(
            _ =>
            {
                requestCount++;
                return CreateOkResponse(DefaultPayload);
            },
            cacheTtlSeconds: 60,
            timeProvider: timeProvider);

        _ = provider.GetExchangeRates([new Currency("USD")]).ToArray();
        timeProvider.Advance(TimeSpan.FromSeconds(30));
        _ = provider.GetExchangeRates([new Currency("EUR")]).ToArray();

        Assert.Equal(1, requestCount);
    }

    [Fact]
    public void GetExchangeRates_after_ttl_expires_refetches()
    {
        FakeTimeProvider timeProvider = new(new DateTimeOffset(2026, 04, 22, 10, 0, 0, TimeSpan.Zero));
        int requestCount = 0;
        ExchangeRateProvider provider = CreateProvider(
            _ =>
            {
                requestCount++;
                return CreateOkResponse(DefaultPayload);
            },
            cacheTtlSeconds: 60,
            timeProvider: timeProvider);

        _ = provider.GetExchangeRates([new Currency("USD")]).ToArray();
        timeProvider.Advance(TimeSpan.FromSeconds(61));
        _ = provider.GetExchangeRates([new Currency("EUR")]).ToArray();

        Assert.Equal(2, requestCount);
    }

    [Fact]
    public void GetExchangeRates_parse_error_preserves_previous_snapshot()
    {
        FakeTimeProvider timeProvider = new(new DateTimeOffset(2026, 04, 22, 10, 0, 0, TimeSpan.Zero));
        DateTimeOffset expectedLastModified = new(2026, 04, 22, 9, 0, 0, TimeSpan.Zero);
        int requestCount = 0;
        List<DateTimeOffset?> ifModifiedSinceValues = [];
        ExchangeRateProvider provider = CreateProvider(
            request =>
            {
                requestCount++;
                ifModifiedSinceValues.Add(request.Headers.IfModifiedSince);
                return requestCount switch
                {
                    1 => CreateOkResponse(DefaultPayload, expectedLastModified),
                    2 => CreateOkResponse("invalid payload"),
                    3 => CreateNotModifiedResponse(),
                    _ => throw new InvalidOperationException("Unexpected request count.")
                };
            },
            cacheTtlSeconds: 60,
            timeProvider: timeProvider);

        _ = provider.GetExchangeRates([new Currency("USD")]).ToArray();
        timeProvider.Advance(TimeSpan.FromSeconds(61));
        Assert.Throws<FormatException>(() => provider.GetExchangeRates([new Currency("USD")]).ToArray());
        ExchangeRate[] rates = provider.GetExchangeRates([new Currency("USD")]).ToArray();

        ExchangeRate rate = Assert.Single(rates);
        Assert.Equal("USD", rate.SourceCurrency.Code);
        Assert.Equal(3, requestCount);
        Assert.Equal(expectedLastModified, ifModifiedSinceValues[1]);
        Assert.Equal(expectedLastModified, ifModifiedSinceValues[2]);
    }

    [Fact]
    public void GetExchangeRates_http_error_preserves_previous_snapshot()
    {
        FakeTimeProvider timeProvider = new(new DateTimeOffset(2026, 04, 22, 10, 0, 0, TimeSpan.Zero));
        DateTimeOffset expectedLastModified = new(2026, 04, 22, 9, 0, 0, TimeSpan.Zero);
        int requestCount = 0;
        List<DateTimeOffset?> ifModifiedSinceValues = [];
        ExchangeRateProvider provider = CreateProvider(
            request =>
            {
                requestCount++;
                ifModifiedSinceValues.Add(request.Headers.IfModifiedSince);
                return requestCount switch
                {
                    1 => CreateOkResponse(DefaultPayload, expectedLastModified),
                    2 => new HttpResponseMessage(HttpStatusCode.BadGateway),
                    3 => CreateNotModifiedResponse(),
                    _ => throw new InvalidOperationException("Unexpected request count.")
                };
            },
            cacheTtlSeconds: 60,
            timeProvider: timeProvider);

        _ = provider.GetExchangeRates([new Currency("USD")]).ToArray();
        timeProvider.Advance(TimeSpan.FromSeconds(61));
        _ = Assert.Throws<HttpRequestException>(() => provider.GetExchangeRates([new Currency("USD")]).ToArray());
        ExchangeRate[] rates = provider.GetExchangeRates([new Currency("USD")]).ToArray();

        ExchangeRate rate = Assert.Single(rates);
        Assert.Equal("USD", rate.SourceCurrency.Code);
        Assert.Equal(3, requestCount);
        Assert.Equal(expectedLastModified, ifModifiedSinceValues[1]);
        Assert.Equal(expectedLastModified, ifModifiedSinceValues[2]);
    }

    [Fact]
    public void GetExchangeRates_refresh_failure_does_not_corrupt_existing_snapshot()
    {
        FakeTimeProvider timeProvider = new(new DateTimeOffset(2026, 04, 22, 10, 0, 0, TimeSpan.Zero));
        DateTimeOffset expectedLastModified = new(2026, 04, 22, 9, 0, 0, TimeSpan.Zero);
        int requestCount = 0;
        List<DateTimeOffset?> ifModifiedSinceValues = [];
        ExchangeRateProvider provider = CreateProvider(
            request =>
            {
                requestCount++;
                ifModifiedSinceValues.Add(request.Headers.IfModifiedSince);
                return requestCount switch
                {
                    1 => CreateOkResponse(DefaultPayload, expectedLastModified),
                    2 => new HttpResponseMessage(HttpStatusCode.BadGateway),
                    3 => CreateOkResponse(UpdatedPayload),
                    _ => throw new InvalidOperationException("Unexpected request count.")
                };
            },
            cacheTtlSeconds: 60,
            timeProvider: timeProvider);

        _ = provider.GetExchangeRates([new Currency("USD")]).ToArray();
        timeProvider.Advance(TimeSpan.FromSeconds(61));
        _ = Assert.Throws<HttpRequestException>(() => provider.GetExchangeRates([new Currency("USD")]).ToArray());
        ExchangeRate[] refreshedRates = provider.GetExchangeRates([new Currency("USD")]).ToArray();

        ExchangeRate rate = Assert.Single(refreshedRates);
        Assert.Equal(23.000m, rate.Value);
        Assert.Equal(3, requestCount);
        Assert.Equal(expectedLastModified, ifModifiedSinceValues[1]);
        Assert.Equal(expectedLastModified, ifModifiedSinceValues[2]);
    }

    [Fact]
    public void GetExchangeRates_ttl_boundary_exact_match_is_treated_as_expired()
    {
        FakeTimeProvider timeProvider = new(new DateTimeOffset(2026, 04, 22, 10, 0, 0, TimeSpan.Zero));
        int requestCount = 0;
        ExchangeRateProvider provider = CreateProvider(
            _ =>
            {
                requestCount++;
                return CreateOkResponse(DefaultPayload);
            },
            cacheTtlSeconds: 60,
            timeProvider: timeProvider);

        _ = provider.GetExchangeRates([new Currency("USD")]).ToArray();
        timeProvider.Advance(TimeSpan.FromSeconds(60));
        _ = provider.GetExchangeRates([new Currency("USD")]).ToArray();

        Assert.Equal(2, requestCount);
    }

    [Fact]
    public void GetExchangeRates_response_without_last_modified_does_not_inherit_previous_value()
    {
        FakeTimeProvider timeProvider = new(new DateTimeOffset(2026, 04, 22, 10, 0, 0, TimeSpan.Zero));
        DateTimeOffset firstLastModified = new(2026, 04, 22, 9, 0, 0, TimeSpan.Zero);
        int requestCount = 0;
        List<DateTimeOffset?> ifModifiedSinceValues = [];
        ExchangeRateProvider provider = CreateProvider(
            request =>
            {
                requestCount++;
                ifModifiedSinceValues.Add(request.Headers.IfModifiedSince);
                return requestCount switch
                {
                    1 => CreateOkResponse(DefaultPayload, firstLastModified),
                    2 => CreateOkResponse(UpdatedPayload),
                    3 => CreateOkResponse(UpdatedPayload),
                    _ => throw new InvalidOperationException("Unexpected request count.")
                };
            },
            cacheTtlSeconds: 60,
            timeProvider: timeProvider);

        _ = provider.GetExchangeRates([new Currency("USD")]).ToArray();
        timeProvider.Advance(TimeSpan.FromSeconds(61));
        _ = provider.GetExchangeRates([new Currency("USD")]).ToArray();
        timeProvider.Advance(TimeSpan.FromSeconds(61));
        _ = provider.GetExchangeRates([new Currency("USD")]).ToArray();

        Assert.Equal(3, requestCount);
        Assert.Null(ifModifiedSinceValues[0]);
        Assert.Equal(firstLastModified, ifModifiedSinceValues[1]);
        Assert.Null(ifModifiedSinceValues[2]);
    }

    private static ExchangeRateProvider CreateProvider(Func<HttpRequestMessage, HttpResponseMessage> onSend)
    {
        return CreateProvider(onSend, cacheTtlSeconds: 3600);
    }

    private static ExchangeRateProvider CreateProvider(
        Func<HttpRequestMessage, HttpResponseMessage> onSend,
        int cacheTtlSeconds)
    {
        return CreateProvider(
            new FakeHttpClientFactory(new HttpClient(new DelegateHttpMessageHandler(onSend))),
            cacheTtlSeconds,
            timeProvider: null);
    }

    private static ExchangeRateProvider CreateProvider(FakeHttpClientFactory factory)
    {
        return CreateProvider(factory, cacheTtlSeconds: 3600, timeProvider: null);
    }

    private static ExchangeRateProvider CreateProvider(
        Func<HttpRequestMessage, HttpResponseMessage> onSend,
        int cacheTtlSeconds,
        FakeTimeProvider timeProvider)
    {
        return CreateProvider(
            new FakeHttpClientFactory(new HttpClient(new DelegateHttpMessageHandler(onSend))),
            cacheTtlSeconds,
            timeProvider);
    }

    private static ExchangeRateProvider CreateProvider(
        FakeHttpClientFactory factory,
        int cacheTtlSeconds,
        FakeTimeProvider? timeProvider)
    {
        return new ExchangeRateProvider(
            factory,
            Options.Create(new CnbOptions
            {
                Url = "https://example.test/cnb.txt",
                CacheTtlSeconds = cacheTtlSeconds
            }),
            NullLogger<ExchangeRateProvider>.Instance,
            timeProvider);
    }

    private static HttpResponseMessage CreateOkResponse(string content, DateTimeOffset? lastModified = null)
    {
        HttpResponseMessage response = new(HttpStatusCode.OK)
        {
            Content = new StringContent(content)
        };

        if (lastModified is not null)
        {
            response.Content.Headers.LastModified = lastModified;
        }

        return response;
    }

    private static HttpResponseMessage CreateNotModifiedResponse()
    {
        return new HttpResponseMessage(HttpStatusCode.NotModified);
    }

    private const string DefaultPayload = """
        22.04.2026 #78
        Country|Currency|Amount|Code|Rate
        Australia|dollar|1|AUD|14,123
        Eurozone|euro|1|EUR|24,560
        United States|dollar|1|USD|22,640
        """;

    private const string UpdatedPayload = """
        22.04.2026 #79
        Country|Currency|Amount|Code|Rate
        Australia|dollar|1|AUD|14,500
        Eurozone|euro|1|EUR|24,800
        United States|dollar|1|USD|23,000
        """;

    private sealed class FakeHttpClientFactory(HttpClient client) : IHttpClientFactory
    {
        public int CreateClientCalls { get; private set; }

        public HttpClient CreateClient(string name)
        {
            CreateClientCalls++;
            return client;
        }
    }

    private sealed class DelegateHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> onSend) : HttpMessageHandler
    {
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return onSend(request);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return System.Threading.Tasks.Task.FromResult(Send(request, cancellationToken));
        }
    }
}
