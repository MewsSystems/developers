using System.Diagnostics.CodeAnalysis;
using System.Net;
using ExchangeRateUpdater.CNBRateProvider.Client;
using ExchangeRateUpdater.Domain.Models;
using RichardSzalay.MockHttp;
using Xunit;

namespace ExchangeRateUpdater.CNBRateProvider.UnitTests.Client;

[SuppressMessage("Design", "CA1063", Justification = "No need for tests.")]
public class CnbClientShould : IDisposable
{
    private readonly MockHttpMessageHandler _messageHandler;

    private static readonly Uri CbnApiBaseAddress = new Uri("https://api.domain.cz");

    public CnbClientShould()
    {
        _messageHandler = new MockHttpMessageHandler();
    }

    public void Dispose() => _messageHandler.Dispose();

    [Fact]
    public async Task ReturnDailyExchangeRatesOnSuccess()
    {
        // arrange
        var todayDate = DateTime.UtcNow.Date;
        var expectedAudToCzkCurrencyPair = new CurrencyPair(Currency.FromString("AUD"), Currency.FromString("CZK"));
        var expectedHufToCzkCurrencyPair = new CurrencyPair(Currency.FromString("HUF"), Currency.FromString("CZK"));
        var expectedAudRate = 15.858m;
        var expectedHufRate = 0.06374m;

        _messageHandler
            .When(HttpMethod.Get, $"{CbnApiBaseAddress}cnbapi/exrates/daily?date={todayDate:yyyy-MM-dd}&lang=EN")
            .Respond(
                HttpStatusCode.OK,
                "application/json",
                @"{
                  ""rates"": [
                    {
                      ""validFor"": ""2019-05-17"",
                      ""order"": 94,
                      ""country"": ""Australia"",
                      ""currency"": ""dollar"",
                      ""amount"": 1,
                      ""currencyCode"": ""AUD"",
                      ""rate"": 15.858
                    },
                    {
                      ""validFor"": ""2019-05-17"",
                      ""order"": 94,
                      ""country"": ""Hungary"",
                      ""currency"": ""forint"",
                      ""amount"": 100,
                      ""currencyCode"": ""HUF"",
                      ""rate"": 6.374
                    }
                  ]
                  }");

        using var httpClient = new HttpClient(_messageHandler)
        {
            BaseAddress = CbnApiBaseAddress
        };

        var client = new CnbClient(httpClient);

        // act
        var dailyExchangeRateResult = await client.GetDailyExchangeRateToCzk(todayDate, CancellationToken.None);

        // assert
        Assert.True(dailyExchangeRateResult.IsSuccess);

        var exchangeRates = dailyExchangeRateResult.Value;
        Assert.Equal(2, exchangeRates.Count());

        var audExchangeRate = exchangeRates.First();
        Assert.Equal(expectedAudRate, audExchangeRate.Value);
        Assert.Equal(expectedAudToCzkCurrencyPair, audExchangeRate.CurrencyPair);

        var hufExchangeRate = exchangeRates.Last();
        Assert.Equal(expectedHufRate, hufExchangeRate.Value);
        Assert.Equal(expectedHufToCzkCurrencyPair, hufExchangeRate.CurrencyPair);
    }

    [Theory]
    [MemberData(nameof(GenerateDifferentRates))]
    public async Task CalculateRateBasedOnAmount(string rate, int amount, decimal expectedRate)
    {
        // arrange
        var todayDate = DateTime.UtcNow.Date;

        _messageHandler
            .When(HttpMethod.Get, $"{CbnApiBaseAddress}cnbapi/exrates/daily?date={todayDate:yyyy-MM-dd}&lang=EN")
            .Respond(
                HttpStatusCode.OK,
                "application/json",
                $$"""
                {
                    "rates": [
                    {
                      "validFor": "2019-05-17",
                      "order": 94,
                      "country": "Australia",
                      "currency": "dollar",
                      "amount": {{amount}},
                      "currencyCode": "AUD",
                      "rate": {{rate}}
                    }]
                 }
                """);

        using var httpClient = new HttpClient(_messageHandler)
        {
            BaseAddress = CbnApiBaseAddress
        };

        var client = new CnbClient(httpClient);

        // act
        var dailyExchangeRateResult = await client.GetDailyExchangeRateToCzk(todayDate, CancellationToken.None);

        // assert
        Assert.True(dailyExchangeRateResult.IsSuccess);

        var exchangeRates = dailyExchangeRateResult.Value;
        Assert.Equal(1, exchangeRates.Count());
        Assert.Equal(expectedRate, exchangeRates.First().Value);
    }

    [Fact]
    public async Task ReturnSuccessWhenNoExchangeRatesAreProvided()
    {
        // arrange
        var todayDate = DateTime.UtcNow.Date;

        _messageHandler
            .When(HttpMethod.Get, $"{CbnApiBaseAddress}cnbapi/exrates/daily?date={todayDate:yyyy-MM-dd}&lang=EN")
            .Respond(
                HttpStatusCode.OK,
                "application/json",
                @"{""rates"": []}");

        using var httpClient = new HttpClient(_messageHandler)
        {
            BaseAddress = CbnApiBaseAddress
        };

        var client = new CnbClient(httpClient);

        // act
        var dailyExchangeRateResult = await client.GetDailyExchangeRateToCzk(todayDate, CancellationToken.None);

        // assert
        Assert.True(dailyExchangeRateResult.IsSuccess);

        var exchangeRates = dailyExchangeRateResult.Value;
        Assert.Empty(exchangeRates);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task FailOnApiError(HttpStatusCode statusCode)
    {
        // arrange
        var todayDate = DateTime.UtcNow.Date;

        _messageHandler
            .When(HttpMethod.Get, $"{CbnApiBaseAddress}cnbapi/exrates/daily?date={todayDate:yyyy-MM-dd}&lang=EN")
            .Respond(statusCode);

        using var httpClient = new HttpClient(_messageHandler)
        {
            BaseAddress = CbnApiBaseAddress
        };

        var client = new CnbClient(httpClient);

        // act
        var dailyExchangeRateResult = await client.GetDailyExchangeRateToCzk(todayDate, CancellationToken.None);

        // assert
        Assert.False(dailyExchangeRateResult.IsSuccess);
    }

    public static IEnumerable<object[]> GenerateDifferentRates()
    {
        // different casing
        yield return new object[] { "15.858", 1, 15.858m };
        yield return new object[] { "6.374", 100, 0.06374m };
        yield return new object[] { "0.0", 100, 0.0m };
    }
}
