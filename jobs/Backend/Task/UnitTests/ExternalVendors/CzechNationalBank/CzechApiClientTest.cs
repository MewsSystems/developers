using System;
using System.Net;
using System.Net.Http;
using ExchangeRateUpdater.ExternalVendors.CzechNationalBank;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using Xunit;

namespace UnitTests.ExternalVendors.CzechNationalBank;

[TestSubject(typeof(CzechApiClient))]
public class CzechApiClientTest
{
    private readonly Mock<HttpMessageHandler> _handler;
    private readonly CzechApiClient _client;

    public CzechApiClientTest()
    {
        _handler = new Mock<HttpMessageHandler>();
        var factory = _handler.CreateClientFactory();

        Mock.Get(factory).Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(() =>
            {
                var client = _handler.CreateClient();
                client.BaseAddress = new Uri("http://localhost");
                return client;
            });

        Mock<ILogger<CzechApiClient>> logger = new();
        _client = new CzechApiClient(factory, logger.Object);
    }

    [Fact]
    public async void InternalServerError()
    {
        _handler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.InternalServerError);
        var rates = await _client.GetDailyExchangeRates();
        Assert.Empty(rates.Rates);
    }
}