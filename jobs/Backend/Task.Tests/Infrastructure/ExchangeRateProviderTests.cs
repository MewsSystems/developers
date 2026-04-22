using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
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

    private static ExchangeRateProvider CreateProvider(Func<HttpRequestMessage, HttpResponseMessage> onSend)
    {
        return CreateProvider(new FakeHttpClientFactory(new HttpClient(new DelegateHttpMessageHandler(onSend))));
    }

    private static ExchangeRateProvider CreateProvider(FakeHttpClientFactory factory)
    {
        return new ExchangeRateProvider(
            factory,
            Options.Create(new CnbOptions
            {
                Url = "https://example.test/cnb.txt",
                CacheTtlSeconds = 3600
            }),
            NullLogger<ExchangeRateProvider>.Instance);
    }

    private static HttpResponseMessage CreateOkResponse(string content)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(content)
        };
    }

    private const string DefaultPayload = """
        22.04.2026 #78
        Country|Currency|Amount|Code|Rate
        Australia|dollar|1|AUD|14,123
        Eurozone|euro|1|EUR|24,560
        United States|dollar|1|USD|22,640
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
