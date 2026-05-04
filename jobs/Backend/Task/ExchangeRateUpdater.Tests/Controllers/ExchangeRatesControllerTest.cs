using ExchangeRateUpdater.Abstractions.Interfaces;
using ExchangeRateUpdater.Abstractions.Model;
using ExchangeRateUpdater.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ExchangeRateUpdater.Tests.Controllers;

[TestFixture]
[TestOf(typeof(ExchangeRatesController))]
public class ExchangeRatesControllerTest
{
    private IExchangeRatesService serviceMock;
    private ILogger<ExchangeRatesController> loggerMock;
    private ExchangeRatesController controller;

    [SetUp]
    public void SetUp()
    {
        serviceMock = Substitute.For<IExchangeRatesService>();
        loggerMock = Substitute.For<ILogger<ExchangeRatesController>>();
        controller = new ExchangeRatesController(serviceMock, loggerMock);
    }

    [TearDown]
    public void TearDown()
    {
        serviceMock.ClearReceivedCalls();
        loggerMock.ClearReceivedCalls();
    }

    [Test]
    public async Task Get_ReturnsOkWithRates_WhenServiceReturnsRates()
    {
        // Arrange
        const string codes = "USD,EUR";
        var expected = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 23.5m),
            new(new Currency("EUR"), new Currency("CZK"), 25.0m)
        };
        serviceMock.GetRates(Arg.Any<IList<string>>()).Returns(expected);

        // Act
        var result = await controller.Get(codes);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult!.Value, Is.EqualTo(expected));
        await serviceMock.Received(1).GetRates(Arg.Is<IList<string>>(l => l.Count == 2 && l[0] == "USD" && l[1] == "EUR"));
        loggerMock.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("USD,EUR")),
            null,
            Arg.Any<Func<object, Exception, string>>()!);
    }

    [Test]
    public void Get_ThrowsException_WhenServiceThrows()
    {
        // Arrange
        const string codes = "USD";
        serviceMock.GetRates(Arg.Any<IList<string>>()).Throws(new InvalidOperationException());

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => controller.Get(codes));
        serviceMock.Received(1).GetRates(Arg.Any<IList<string>>());
    }
}