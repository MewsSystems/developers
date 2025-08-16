using System.Net;
using Moq;
using Moq.Protected;
using NLog;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    private List<Currency> _currencies;
    private Mock<HttpMessageHandler> _mock;

    [SetUp]
    public void Setup()
    {
        _mock = new Mock<HttpMessageHandler>();
        _currencies = new List<Currency> { new("BRL") };
    }

    [Test]
    public async Task GetExchangeRates()
    {
        _mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{
                    ""rates"": [
                        {""currencyCode"": ""BRL"", ""rate"": 5.796, ""amount"": 1},
                        {""currencyCode"": ""USD"", ""rate"": 23.234, ""amount"": 1},
                        {""currencyCode"": ""EUR"", ""rate"": 24.987, ""amount"": 1}
                    ]
                }")
            });

        var client = new HttpClient(_mock.Object);
        var logger = LogManager.GetCurrentClassLogger();
        var provider = new ExchangeRateProvider(client, logger);

        var data = await provider.GetExchangeRates(_currencies);

        Assert.That(data, Has.Exactly(1).Items);
        Assert.That(data,
            Has.Some.Matches<ExchangeRate>(x => x.SourceCurrency.Code == "BRL" && x.Value == 5.796m && x.Amount == 1));
        Assert.Pass();
    }


    [Test]
    public async Task GetExchangeRatesForMultipleCurrencies()
    {
        _currencies.Add(new Currency("CZK"));
        _currencies.Add(new Currency("USD"));
        _currencies.Add(new Currency("SEK"));
        _currencies.Add(new Currency("PLN"));

        _mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{
                    ""rates"": [
                        {""currencyCode"": ""BRL"", ""rate"": 5.796, ""amount"": 1},
                        {""currencyCode"": ""USD"", ""rate"": 23.234, ""amount"": 1},
                        {""currencyCode"": ""EUR"", ""rate"": 24.987, ""amount"": 1}
                    ]
                }")
            });

        var client = new HttpClient(_mock.Object);
        var logger = LogManager.GetCurrentClassLogger();
        var provider = new ExchangeRateProvider(client, logger);

        var data = await provider.GetExchangeRates(_currencies);

        Assert.That(data, Has.Exactly(2).Items);
        Assert.That(data,
            Has.Some.Matches<ExchangeRate>(x => x.SourceCurrency.Code == "BRL" && x.Value == 5.796m && x.Amount == 1));
        Assert.That(data,
            Has.Some.Matches<ExchangeRate>(x => x.SourceCurrency.Code == "USD" && x.Value == 23.234m && x.Amount == 1));
        Assert.That(data,
            Has.None.Matches<ExchangeRate>(x => x.SourceCurrency.Code == "EUR" && x.Value == 24.987m && x.Amount == 1));
        Assert.Pass();
    }

    [Test] public async Task GetExchangeRatesForEmptyResult()
    {
        _mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{
                    ""rates"": []
                }")
            });

        var client = new HttpClient(_mock.Object);
        var logger = LogManager.GetCurrentClassLogger();
        var provider = new ExchangeRateProvider(client, logger);

        var data = await provider.GetExchangeRates(_currencies);

        Assert.That(data, Has.Exactly(0).Items);
        Assert.That(data,
            Has.None.Matches<ExchangeRate>(x => x.SourceCurrency.Code == "BRL" && x.Value == 5.796m && x.Amount == 1));
        Assert.Pass();
    }

    [Test]
    public async Task GetExchangeRatesServerError()
    {
        _mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        var client = new HttpClient(_mock.Object);
        var logger = LogManager.GetCurrentClassLogger();
        var provider = new ExchangeRateProvider(client, logger);

        var exception = Assert.ThrowsAsync<HttpRequestException>(() => provider.GetExchangeRates(_currencies));
        Assert.That(exception, Is.Not.Null);
    }
}