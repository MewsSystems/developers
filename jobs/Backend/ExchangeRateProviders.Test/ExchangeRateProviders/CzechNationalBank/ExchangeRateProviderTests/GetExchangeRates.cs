using ExchangeRateUpdater.ExchangeRateProviders.CzechNationalBank;
using ExchangeRateUpdater.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using System.Net;

namespace ExchangeRateUpdater.Test.ExchangeRateProviders.CzechNationalBank.ExchangeRateProviderTests;

public class GetExchangeRates
{
    private readonly Currency CZK = new("Czech Republic", "koruna", "CZK");
    private readonly Currency GBP = new("United Kingdom", "pound", "GBP");
    private readonly Currency USD = new("USA", "dollar", "USD");

    [Fact]
    public async Task GivenExchangeRateRequestFails_ThenFailedResultIsReturned()
    {
        var exchangeRateProvider = new CzechNationalBankExchangeRateProvider(MockHttpClient(HttpStatusCode.NotFound),
            new MockDateTimeProvider(new DateTime(2000, 1, 1)),
            MockConfiguration());

        var getExchangeRateResult = await exchangeRateProvider.GetExchangeRates(["GBP", "USD", "EUR"]);

        getExchangeRateResult.HasSucceeded.Should().BeFalse();
        getExchangeRateResult.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        getExchangeRateResult.Value.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task GiveExchangeRateRequestSucceeded_ThenRequestedExchangeRatesAreReturned()
    {
        var httpContent = new StringContent(
            "{\"rates\": [{\"validFor\": \"2019-05-17\",\"order\": 94,\"country\": \"Australia\",\"currency\": \"dollar\",\"amount\": 1,\"currencyCode\": \"AUD\",\"rate\": 15.858}," +
            "{\"validFor\": \"2019-05-17\",\"order\": 94,\"country\": \"United Kingdom\",\"currency\": \"pound\",\"amount\": 1,\"currencyCode\": \"GBP\",\"rate\": 29.395}," +
            "{\"validFor\": \"2019-05-17\",\"order\": 94,\"country\": \"USA\",\"currency\": \"dollar\",\"amount\": 1,\"currencyCode\": \"USD\",\"rate\": 23.048}]}");

        var exchangeRateProvider = new CzechNationalBankExchangeRateProvider(MockHttpClient(HttpStatusCode.OK, httpContent),
            new MockDateTimeProvider(new DateTime(2000, 1, 1)),
            MockConfiguration());

        var getExchangeRateResult = await exchangeRateProvider.GetExchangeRates(["GBP", "USD", "EUR"]);

        getExchangeRateResult.HasSucceeded.Should().BeTrue();
        getExchangeRateResult.ErrorMessage.Should().BeNullOrWhiteSpace();
        getExchangeRateResult.Value.Should().BeEquivalentTo(new[]
        {
            new ExchangeRate(CZK, GBP, 29.395m),
            new ExchangeRate(CZK, USD, 23.048m)
        });
    }

    private static HttpClient MockHttpClient(HttpStatusCode httpStatusCode, HttpContent? httpContent = null)
    {
        return new HttpClient(new MockHttpMessageHandler(httpStatusCode, httpContent));
    }

    private static IOptions<CzechNationalBankExchangeRateProviderConfiguration> MockConfiguration()
    {
        return Options.Create(new CzechNationalBankExchangeRateProviderConfiguration
            { ApiDateFormat = "yyyy-MM-dd", ApiUrl = "https://example.org" });
    }
}