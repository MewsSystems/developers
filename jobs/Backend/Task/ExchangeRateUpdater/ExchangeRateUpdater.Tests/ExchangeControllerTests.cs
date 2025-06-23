using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class ExchangeControllerTests
{
    private ExchangeRatesController _controller;
    private Mock<IExchangeRateService> _mockService;
    private Mock<ILogger<ExchangeRatesController>> _mockLogger;

    [SetUp]
    public void SetUp()
    {
        _mockService = new Mock<IExchangeRateService>();
        _mockLogger = new Mock<ILogger<ExchangeRatesController>>();
        _controller = new ExchangeRatesController(_mockService.Object, _mockLogger.Object);        
    }

    [Test]
    public async Task GivenNullExchangeRate_ShouldReturnBadRequest()
    {
        // Arrange            
        var exchangeRatesToRequest = new ExchangeRateRequestDto() { Date = new DateTime(2025, 01, 20) };

        // Act
        var result = await _controller.GetDailyExchangeRatesAsync(exchangeRatesToRequest, new CancellationToken());

        // Assert            
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);  // Verifica que sea un BadRequestObjectResult
        Assert.AreEqual(400, badRequestResult.StatusCode);  // Verifica que el código de estado sea 400
        Assert.AreEqual("Exchange rate(s) cannot be empty or null", badRequestResult.Value);  // Verifica el mensaje
    }

    [Test]
    public async Task GivenEmptyExchangeRate_ShouldReturnBadRequest()
    {
        // Arrange            
        var exchangeRatesToRequest = new ExchangeRateRequestDto()
        {
            Date = new DateTime(2025, 01, 20),
            ExchangeRatesDetails = new List<ExchangeRateRequest>()
        };

        // Act
        var result = await _controller.GetDailyExchangeRatesAsync(exchangeRatesToRequest, new CancellationToken());

        // Assert            
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);  // Verifica que sea un BadRequestObjectResult
        Assert.AreEqual(400, badRequestResult.StatusCode);  // Verifica que el código de estado sea 400
        Assert.AreEqual("Exchange rate(s) cannot be empty or null", badRequestResult.Value);  // Verifica el mensaje
    }

    [Test]
    public async Task GivenValidExchangeRate_ShouldReturnExchangeRate()
    {
        // Arrange            
        var request = new ExchangeRateRequestDto()
        {
            Date = new DateTime(2025, 01, 20),
            ExchangeRatesDetails = new List<ExchangeRateRequest>()
            {
                new ExchangeRateRequest()
                {
                    SourceCurrency = new Currency("CZK"),
                    TargetCurrency = new Currency("USD")
                }
            }
        };
        var resultado = new List<ExchangeRateResultDto>() {
            new ExchangeRateResultDto(targetCurrency: new Currency("USD"), sourceCurrency: new Currency("CZK"), value:10, date: new DateTime(2025, 01, 20))
        };

        _mockService.Setup(s => s.GetExchangeRates(request.ExchangeRatesDetails, request.Date, new CancellationToken())).ReturnsAsync(resultado);

        //// Act
        var result = await _controller.GetDailyExchangeRatesAsync(request, new CancellationToken());

        // Assert            
        var OkResult = result as OkObjectResult;
        Assert.IsNotNull(OkResult);
        Assert.AreEqual(200, OkResult.StatusCode);

        var rates = OkResult.Value as List<ExchangeRateResultDto>;
        Assert.IsNotNull(rates);
        Assert.IsNotEmpty(rates);
        Assert.IsTrue(rates.Any());
    }

    [Test]
    public async Task GivenInalidExchangeRate_ShouldReturnEmptyList()
    {
        // Arrange            
        var request = new ExchangeRateRequestDto()
        {
            Date = new DateTime(2025, 01, 20),
            ExchangeRatesDetails = new List<ExchangeRateRequest>()
            {
                new ExchangeRateRequest()
                {
                    SourceCurrency = new Currency("CZK"),
                    TargetCurrency = new Currency("ABC")
                }
            }
        };
        var resultado = new List<ExchangeRateResultDto>();

        _mockService.Setup(s => s.GetExchangeRates(request.ExchangeRatesDetails, request.Date, new CancellationToken())).ReturnsAsync(resultado);

        //// Act
        var result = await _controller.GetDailyExchangeRatesAsync(request, new CancellationToken());

        // Assert            
        var OkResult = result as OkObjectResult;
        Assert.IsNotNull(OkResult);
        Assert.AreEqual(200, OkResult.StatusCode);

        var rates = OkResult.Value as List<ExchangeRateResultDto>;
        Assert.IsNotNull(rates);
        Assert.IsEmpty(rates);
        Assert.IsFalse(rates.Any());
    }
}