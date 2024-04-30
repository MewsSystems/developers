using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Castle.Core.Logging;
using ExchangeRateUpdater;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExchangeRateUpdaterTests;

public class ExchangeRateProviderTests
{
    [Fact]
    public void GetExchangeRates_NoCurrenciesPassedAndAClientResponseWithRates_ResponseIsEmpty()
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

        var rates = provider.GetExchangeRates(Array.Empty<Currency>());

        Assert.Empty(rates);
    }

    [Fact]
    public void GetExchangeRates_WithAnAUDCurrencyRequestAndAClientResponseThatContainsAUDRate_ResponseContainsExpectedRate()
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

        var rates = provider.GetExchangeRates(new[] { new Currency(AUDCode) });

        Assert.NotEmpty(rates);
        Assert.Contains(rates, rate => rate.TargetCurrency.Code == AUDCode && rate.Value == Rate);
    }

    [Fact]
    public void GetExchangeRates_WithAnAUDCurrencyRequestAndAClientResponseThatDoesNotContainAUDRate_ResponseIsEmpty()
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

        var rates = provider.GetExchangeRates(new[] { new Currency(AUDCode) });

        Assert.Empty(rates);
    }

    [Fact]
    public void GetExchangeRates_WithTwoRequestCurrenciesThatAreContainedIntheClientResponse_ResponseContainsExpectedRates()
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

        var rates = provider.GetExchangeRates(new[] { new Currency(AUDCode), new Currency(CADCode) });

        Assert.NotEmpty(rates);
        Assert.Equal(2, rates.Count());
        Assert.Contains(rates, rate => rate.TargetCurrency.Code == AUDCode && rate.Value == AUDRate);
        Assert.Contains(rates, rate => rate.TargetCurrency.Code == CADCode && rate.Value == CADRate);
    }

    private static ExchangeRateProvider CreateExchangeRateProvider(string response)
    {
        var httpHandler = new MockHttpMessageHandler(response);
        var httpClient = new HttpClient(httpHandler);

        var provider = new ExchangeRateProvider(httpClient, Substitute.For<ILogger<ExchangeRateProvider>>());
        return provider;
    }

    class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;

        public MockHttpMessageHandler(string response)
        {
            _response = response;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_response)
            };
        }
    }
}


