using ExchangeRateUpdater.DTOs;
using ExchangeRateUpdater.Services;
using Moq;
using Moq.Protected;
using System.Net;

namespace TestExchangeRateUpdater.Tests.Services;

[TestFixture]
public class ExchangeRateServiceTests
{
    private ExchangeRateService _exchangeRateService;

    [SetUp]
    public void Setup()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent
                (
                    "{\"rates\": [{\"validFor\": \"2025-01-11\", \"order\": 1, \"currency\": \"Currency\", \"country\": \"Country\", \"amount\": 1, \"currencyCode\": \"CUR\", \"rate\": 10.00}]}"
                )
            });
        var httpClient = new HttpClient(handlerMock.Object);
        httpClient.BaseAddress = new Uri("https://baseaddress/");
        _exchangeRateService = new ExchangeRateService(httpClient);

    }

    [Test]
    public void GetExchangeRates_ShouldReturn_CorrectExchangeRatesDTO()
    {
        // Arrange
        var expected = new ExchangeRatesDTO
        {
            Rates = new List<ExchangeRateDTO>
            {
                new ()
                {
                    CurrencyCode = "CUR",
                    Rate = 10.00M
                }
            }
        };

        // Act
        var actual = _exchangeRateService.GetExchangeRates().Result;

        // Assert
        Assert.That(actual.Rates, Is.EqualTo(expected.Rates));
    }

    [Test]
    public void GetExchangeRates_ShouldThrowException_WhenResponseIsNotSuccessful()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });
        var httpClient = new HttpClient(handlerMock.Object);
        httpClient.BaseAddress = new Uri("https://baseaddress/");

        _exchangeRateService = new ExchangeRateService(httpClient);

        // Assert
        Assert.Multiple(() =>
        {
            var exception = Assert.ThrowsAsync<Exception>(() => _exchangeRateService.GetExchangeRates());
            Assert.That(exception.Message, Is.EqualTo("Failed to get exchange rates. Status code: BadRequest"));
        });
    }
}
