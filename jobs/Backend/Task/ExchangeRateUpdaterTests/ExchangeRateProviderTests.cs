using System.Net;
using ExchangeRateUpdater;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace ExchangeRateUpdaterTests;

public class ExchangeRateProviderTests
{
    private readonly ILogger<ExchangeRateProvider> _logger;
    private readonly string _testResponse;
    private readonly IOptions<ExchangeRateProviderOptions> _options;

    public ExchangeRateProviderTests()
    {
        _logger = Substitute.For<ILogger<ExchangeRateProvider>>();
        _testResponse = @"{
        ""rates"": [
            {
                ""validFor"": ""2024-04-26"",
                ""order"": 82,
                ""country"": ""AUD"",
                ""currency"": ""dollar"",
                ""amount"": 1,
                ""currencyCode"": ""AUD"",
                ""rate"": 30
            }
        ]}";
        _options = Substitute.For<IOptions<ExchangeRateProviderOptions>>();
        var providerOptions = new ExchangeRateProviderOptions { ApiRequestUri = "https://www.google.com" };
        _options.Value.Returns(providerOptions);
    }

    [Fact]
    public async Task GetExchangeRates_NoCurrenciesPassedAndAClientResponseWithRates_ResponseIsEmpty()
    {
        var response = @"{
        ""rates"": [
            {
                ""validFor"": ""2024-04-26"",
                ""order"": 82,
                ""country"": ""Australia"",
                ""currency"": ""dollar"",
                ""amount"": 1,
                ""currencyCode"": ""AUD"",
                ""rate"": 15.35
            }
        ]}";
        ExchangeRateProvider provider = CreateExchangeRateProvider(response);

        var rates = await provider.GetExchangeRates(Array.Empty<Currency>());

        Assert.Empty(rates);
    }

    [Fact]
    public async Task GetExchangeRates_WithAnAUDCurrencyRequestAndAClientResponseThatContainsAUDRate_ResponseContainsExpectedRate()
    {
        const string AUDCode = "AUD";
        const decimal Rate = 15.35M;
        var response = $@"{{
        ""rates"": [
            {{
                ""validFor"": ""2024-04-26"",
                ""order"": 82,
                ""country"": ""Australia"",
                ""currency"": ""dollar"",
                ""amount"": 1,
                ""currencyCode"": ""{AUDCode}"",
                ""rate"": {Rate}
            }}
        ]}}";
        ExchangeRateProvider provider = CreateExchangeRateProvider(response);

        var rates = await provider.GetExchangeRates(new[] { new Currency(AUDCode) });

        Assert.NotEmpty(rates);
        Assert.Contains(rates, rate => rate.TargetCurrency.Code == AUDCode && rate.Value == Rate);
    }

    [Fact]
    public async Task GetExchangeRates_WithAnAUDCurrencyRequestAndAClientResponseThatDoesNotContainAUDRate_ResponseIsEmpty()
    {
        const string AUDCode = "AUD";
        var response = $@"{{
        ""rates"": [
            {{
                ""validFor"": ""2024-04-26"",
                ""order"": 82,
                ""country"": ""Test"",
                ""currency"": ""dollar"",
                ""amount"": 1,
                ""currencyCode"": ""GBP"",
                ""rate"": 30
            }}
        ]}}";
        ExchangeRateProvider provider = CreateExchangeRateProvider(response);

        var rates = await provider.GetExchangeRates(new[] { new Currency(AUDCode) });

        Assert.Empty(rates);
    }

    [Fact]
    public async Task GetExchangeRates_WithTwoRequestCurrenciesThatAreContainedIntheClientResponse_ResponseContainsExpectedRates()
    {
        const string AUDCode = "AUD";
        const decimal AUDRate = 15.35M;
        const string CADCode = "CAD";
        const decimal CADRate = 17.195M;
        var response = $@"{{
        ""rates"": [
            {{
                ""validFor"": ""2024-04-26"",
                ""order"": 82,
                ""country"": ""Australia"",
                ""currency"": ""dollar"",
                ""amount"": 1,
                ""currencyCode"": ""{AUDCode}"",
                ""rate"": {AUDRate}
            }},
            {{
                ""validFor"": ""2024-04-26"",
                ""order"": 82,
                ""country"": ""Canada"",
                ""currency"": ""dollar"",
                ""amount"": 1,
                ""currencyCode"": ""{CADCode}"",
                ""rate"": {CADRate}
            }}
        ]}}";
        ExchangeRateProvider provider = CreateExchangeRateProvider(response);

        var rates = await provider.GetExchangeRates(new[] { new Currency(AUDCode), new Currency(CADCode) });

        Assert.NotEmpty(rates);
        Assert.Equal(2, rates.Count());
        Assert.Contains(rates, rate => rate.TargetCurrency.Code == AUDCode && rate.Value == AUDRate);
        Assert.Contains(rates, rate => rate.TargetCurrency.Code == CADCode && rate.Value == CADRate);
    }

    [Fact]
    public async Task GetExchangeRates_ErrorFromRemoteApi_ExceptionIsThrown()
    {
        var httpClient = new HttpClient(new MockErrorHttpMessageHandler());
        var provider = new ExchangeRateProvider(httpClient, _logger, _options);

        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await provider.GetExchangeRates(new[] { new Currency("AUD") });
        });
    }

    [Fact]
    public async Task GetExchangeRates_ErrorFromRemoteApi_ExceptionIsLogged()
    {
        var httpClient = new HttpClient(new MockErrorHttpMessageHandler());
        var testableLogger = new TestableLogger<ExchangeRateProvider>();
        var provider = new ExchangeRateProvider(httpClient, testableLogger, _options);

        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await provider.GetExchangeRates(new[] { new Currency("AUD") });
        });

        Assert.True(testableLogger.ErrorLogged);
    }

    [Fact]
    public async Task GetExchangeRates_ApiUriFromOptionsIsUsed()
    {
        var providerOptions = new ExchangeRateProviderOptions { ApiRequestUri = "https://www.google.com/" };
        var httpHandler = new MockHttpMessageHandler(_testResponse);
        var httpClient = new HttpClient(httpHandler);
        var provider = new ExchangeRateProvider(httpClient, _logger, Options.Create(providerOptions));

        await provider.GetExchangeRates(new[] { new Currency("AUD") });

        Assert.Same(providerOptions.ApiRequestUri, httpHandler.LastUri?.ToString());
    }

    private ExchangeRateProvider CreateExchangeRateProvider(string? response = null)
    {
        if (response == null)
        {
            response = _testResponse;
        }

        var httpHandler = new MockHttpMessageHandler(response);
        var httpClient = new HttpClient(httpHandler);

        var provider = new ExchangeRateProvider(httpClient, _logger, _options);
        return provider;
    }

    class MockErrorHttpMessageHandler : HttpMessageHandler
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            CancellationToken cancellationToken)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
            };
        }
    }

    class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;
        private Uri? _lastUri;

        public MockHttpMessageHandler(string response)
        {
            _response = response;
        }

        public Uri? LastUri => _lastUri;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            CancellationToken cancellationToken)
        {
            _lastUri = request.RequestUri;
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_response)
            };
        }
    }

    public class TestableLogger<T> : ILogger<T>
    {
        private bool _errorLogged = false;

        public bool ErrorLogged => _errorLogged;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Error)
            {
                _errorLogged = true;
            }
        }
    }
}


