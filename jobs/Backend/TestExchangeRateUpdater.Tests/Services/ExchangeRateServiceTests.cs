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
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"rates\": [{\"validFor\": \"2025-01-11\", \"order\": 1, \"currency\": \"Currency\", \"country\": \"Country\", \"amount\": 1, \"currencyCode\": \"CUR\", \"rate\": 10.00}]}"
                    )
            })
            .Verifiable();
        var httpClient = new HttpClient(handlerMock.Object);
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory
            .Setup(_ => _.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        
        _exchangeRateService = new ExchangeRateService(httpClientFactory.Object);

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
                    ValidFor = "2025-01-11",
                    Order = 1,
                    Currency = "Currency",
                    Country = "Country",
                    Amount = 1,
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
}
