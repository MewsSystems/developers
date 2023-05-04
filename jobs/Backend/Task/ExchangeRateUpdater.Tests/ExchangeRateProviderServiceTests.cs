namespace ExchangeRateUpdater.Domain.Tests.Services;

public class ExchangeRateProviderServiceTests
{
    private readonly Mock<ILogger<ExchangeRateProviderService>> _loggerMock;
    private readonly Mock<IValidator<ExchangeRateRequest>> _validatorMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IHttpClientWrapper> _httpClientMock;
    private readonly Mock<IExchangeRatesParser> _exchangeRatesParserMock;
    private readonly Mock<ICalculatorService> _calculatorMock;

    public ExchangeRateProviderServiceTests()
    {
        _loggerMock = new Mock<ILogger<ExchangeRateProviderService>>();
        _validatorMock = new Mock<IValidator<ExchangeRateRequest>>();
        _configurationMock = new Mock<IConfiguration>();
        _httpClientMock = new Mock<IHttpClientWrapper>();
        _exchangeRatesParserMock = new Mock<IExchangeRatesParser>();
        _calculatorMock = new Mock<ICalculatorService>();
    }

    [Fact]
    public async Task GetExchangeRates_WithValidRequest_ReturnsExchangeRateResponse()
    {
        // Arrange
        var request = new ExchangeRateRequest
        {
            Currencies = new List<Currency> { new Currency("USD"), new Currency("EUR"), new Currency("INR") }
        };

        var exchangeRatesUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        var responseText = Constants.ResponseText;
        _validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ExchangeRatesUrl", exchangeRatesUrl }
            })
            .Build();
        _configurationMock.Setup(c => c.GetSection("ExchangeRatesUrl")).Returns(configuration.GetSection("ExchangeRatesUrl"));
        _httpClientMock.Setup(c => c.GetAsync(exchangeRatesUrl)).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseText) });
        _exchangeRatesParserMock.Setup(p => p.Parse(responseText)).Returns(new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 1, 21.411m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 1, 23.505m),
            new ExchangeRate(new Currency("INR"), new Currency("CZK"), 100, 26.165m)
        });
        _calculatorMock.Setup(c => c.CalculateRates(It.IsAny<List<ExchangeRate>>())).Returns(new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 1, 21.411m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 1, 23.505m),
            new ExchangeRate(new Currency("INR"), new Currency("CZK"), 1, 0.261m)
        });

        var service = new ExchangeRateProviderService(
            _loggerMock.Object,
            _validatorMock.Object,
            _configurationMock.Object,
            _httpClientMock.Object,
            _exchangeRatesParserMock.Object,
            _calculatorMock.Object
        );

        // Act
        var response = await service.GetExchangeRates(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Result);
        Assert.Empty(response.Errors);
        Assert.Equal(3, response.ExchangeRates.Count);
        Assert.Contains(response.ExchangeRates, er => er.SourceCurrency.Code == "USD" && er.TargetCurrency.Code == "CZK" && er.Value == 21.411m);
        Assert.Contains(response.ExchangeRates, er => er.SourceCurrency.Code == "EUR" && er.TargetCurrency.Code == "CZK" && er.Value == 23.505m);
        Assert.Contains(response.ExchangeRates, er => er.SourceCurrency.Code == "INR" && er.TargetCurrency.Code == "CZK" && er.Value == 0.261m);
    }

    [Fact]
    public async Task GetExchangeRates_WithInvalidRequest_ReturnsExchangeRateResponseWithErrors()
    {
        // Arrange
        var request = new ExchangeRateRequest
        {
            Currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") }
        };

        var validationResult = new ValidationResult 
        { 
            Errors = new List<ValidationFailure> 
            { 
                new ValidationFailure 
                { 
                    ErrorMessage = "Invalid request" 
                } 
            } 
        };

        _validatorMock.Setup(v => v.Validate(request)).Returns(validationResult);

        var service = new ExchangeRateProviderService(
            _loggerMock.Object,
            _validatorMock.Object,
            _configurationMock.Object,
            _httpClientMock.Object,
            _exchangeRatesParserMock.Object,
            _calculatorMock.Object
        );

        // Act
        var response = await service.GetExchangeRates(request);

        // Assert
        Assert.NotNull(response);
        Assert.False(response.Result);
        Assert.NotEmpty(response.Errors);
        Assert.Single(response.Errors);
        Assert.Equal("Invalid request", response.Errors.First().Error);
        Assert.Empty(response.ExchangeRates);
    }
}