using ExchangeRateUpdater.ExchangeRate.Controller;
using ExchangeRateUpdater.ExchangeRate.Controller.Model;
using ExchangeRateUpdater.ExchangeRate.Exception;
using ExchangeRateUpdater.ExchangeRate.Model;
using ExchangeRateUpdater.ExchangeRate.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRatesControllerTests
    {
        private readonly Mock<IExchangeRateService> _mockExchangeRateService;
        private readonly Mock<ILogger<ExchangeRatesController>> _mockLogger;
        private readonly ExchangeRatesController _controller;

        public ExchangeRatesControllerTests()
        {
            _mockExchangeRateService = new Mock<IExchangeRateService>();
            _mockLogger = new Mock<ILogger<ExchangeRatesController>>();
            _controller = new ExchangeRatesController(_mockExchangeRateService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForFutureDate()
        {
            // Arrange
            string baseCurrency = "USD";
            string date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("yyyy-MM-dd");
            string language = "EN";
            string targetCurrencies = "EUR,GBP";

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("Invalid date. No exchange rate exists for a future date.", errorResponse.Message);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForInvalidLanguage()
        {
            // Arrange
            string baseCurrency = "USD";
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string language = "FR";
            string targetCurrencies = "EUR,GBP";

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("Invalid language value. Please use 'EN' or 'CZ'.", errorResponse.Message);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsOk_ForValidRequest()
        {
            // Arrange
            string baseCurrency = "USD";
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string language = "EN";
            string targetCurrencies = "EUR,GBP";

            var mockResponse = new FetchDailyExchangeRateResponseInternal(
                new Currency(baseCurrency),
                DateOnly.FromDateTime(DateTime.Today),
                [
                    new("Euro", "EUR", "Europe", 0.85M),
                    new("British Pound", "GBP", "UK", 0.75M)
                ]);

            _mockExchangeRateService
                .Setup(service => service.GetDailyExchangeRatesAsync(It.IsAny<FetchDailyExchangeRateRequestInternal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var successResponse = Assert.IsType<SuccessResponse<FetchDailyExchangeRateResponse>>(okResult.Value);
            Assert.Equal("Daily exchange rate fetched successfully", successResponse.Message);
            Assert.Equal(baseCurrency, successResponse.Data.SourceCurrency);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Today), successResponse.Data.Date);
            Assert.Equal(2, successResponse.Data.ExchangeRates.Count());
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForExchangeRateUpdaterException()
        {
            // Arrange
            string baseCurrency = "USD";
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string language = "EN";
            string targetCurrencies = "EUR,GBP";

            _mockExchangeRateService
                .Setup(service => service.GetDailyExchangeRatesAsync(It.IsAny<FetchDailyExchangeRateRequestInternal>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ExchangeRateUpdaterException("Test exception"));

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Contains("Test exception", errorResponse.Message);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForMissingBaseCurrency()
        {
            // Arrange
            string? baseCurrency = null;
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string language = "EN";
            string targetCurrencies = "EUR,GBP";

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForMissingDate()
        {
            // Arrange
            string baseCurrency = "USD";
            string? date = null;
            string language = "EN";
            string targetCurrencies = "EUR,GBP";

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForMissingLanguage()
        {
            // Arrange
            string baseCurrency = "USD";
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string? language = null;
            string targetCurrencies = "EUR,GBP";

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForMissingTargetCurrencies()
        {
            // Arrange
            string baseCurrency = "USD";
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string language = "EN";
            string? targetCurrencies = null;

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForEmptyTargetCurrencies()
        {
            // Arrange
            string baseCurrency = "USD";
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string language = "EN";
            string targetCurrencies = "";

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsOk_WithEmptyExchangeRates()
        {
            // Arrange
            string baseCurrency = "USD";
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string language = "EN";
            string targetCurrencies = "EUR,GBP";

            var mockResponse = new FetchDailyExchangeRateResponseInternal(
                new Currency(baseCurrency),
                DateOnly.FromDateTime(DateTime.Today),
                []);

            _mockExchangeRateService
                .Setup(service => service.GetDailyExchangeRatesAsync(It.IsAny<FetchDailyExchangeRateRequestInternal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var successResponse = Assert.IsType<SuccessResponse<FetchDailyExchangeRateResponse>>(okResult.Value);
            Assert.Equal("Daily exchange rate fetched successfully", successResponse.Message);
            Assert.Equal(baseCurrency, successResponse.Data.SourceCurrency);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Today), successResponse.Data.Date);
            Assert.Empty(successResponse.Data.ExchangeRates);
        }

        [Fact]
        public async Task GetDailyExchangeRates_RemovesEmptyEntriesAndTrimsWhitespaces()
        {
            // Arrange
            string baseCurrency = "USD";
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string language = "EN";
            string targetCurrencies = " EUR , , GBP , ";

            var mockResponse = new FetchDailyExchangeRateResponseInternal(
                new Currency(baseCurrency),
                DateOnly.FromDateTime(DateTime.Today),
                [
                    new("Euro", "EUR", "Europe", 0.84m),
                    new("British Pound", "GBP", "United Kingdom", 0.75m)
                ]);

            _mockExchangeRateService
                .Setup(service => service.GetDailyExchangeRatesAsync(It.IsAny<FetchDailyExchangeRateRequestInternal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var successResponse = Assert.IsType<SuccessResponse<FetchDailyExchangeRateResponse>>(okResult.Value);
            Assert.Equal("Daily exchange rate fetched successfully", successResponse.Message);
            Assert.Equal(baseCurrency, successResponse.Data.SourceCurrency);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Today), successResponse.Data.Date);
            Assert.Equal(2, successResponse.Data.ExchangeRates.Count());
            Assert.Contains(successResponse.Data.ExchangeRates, rate => rate.CurrencyCode == "EUR");
            Assert.Contains(successResponse.Data.ExchangeRates, rate => rate.CurrencyCode == "GBP");

            // Verify the internal request sent to the service
            _mockExchangeRateService.Verify(service =>
                service.GetDailyExchangeRatesAsync(It.Is<FetchDailyExchangeRateRequestInternal>(req =>
                    req.TargetCurrencies.Count == 2 &&
                    req.TargetCurrencies.Any(c => c.Code == "EUR") &&
                    req.TargetCurrencies.Any(c => c.Code == "GBP")
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForInvalidBaseCurrency(string baseCurrency)
        {
            // Arrange
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string language = "EN";
            string targetCurrencies = "EUR,GBP";

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("baseCurrency cannot be null or empty.", errorResponse.Message);
        }

        [Theory]
        [InlineData("2020-01-01T00:00:00")]
        [InlineData("01/01/2020")]
        [InlineData("20200101")]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForInvalidDateFormat(string date)
        {
            // Arrange
            string baseCurrency = "USD";
            string language = "EN";
            string targetCurrencies = "EUR,GBP";

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("Invalid date format. Please use 'yyyy-MM-dd'.", errorResponse.Message);
        }

        [Theory]
        [InlineData("usd")]
        [InlineData("Usd")]
        [InlineData("USD")]
        public async Task GetDailyExchangeRates_HandlesCaseInsensitiveCurrencyCodes(string baseCurrency)
        {
            // Arrange
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string language = "EN";
            string targetCurrencies = "EUR,GBP";

            var mockResponse = new FetchDailyExchangeRateResponseInternal(
                new Currency(baseCurrency.ToUpper()), // Ensuring the service treats it case insensitively
                DateOnly.FromDateTime(DateTime.Today),
                [
                    new("Euro", "EUR", "Europe", 0.85M),
                    new("British Pound", "GBP", "UK", 0.75M)
                ]);

            _mockExchangeRateService
                .Setup(service => service.GetDailyExchangeRatesAsync(It.IsAny<FetchDailyExchangeRateRequestInternal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var successResponse = Assert.IsType<SuccessResponse<FetchDailyExchangeRateResponse>>(okResult.Value);
            Assert.Equal("Daily exchange rate fetched successfully", successResponse.Message);
            Assert.Equal(baseCurrency.ToUpper(), successResponse.Data.SourceCurrency); // Verify case-insensitivity
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForOverlyLongDate()
        {
            // Arrange
            string baseCurrency = "USD";
            string date = new string('1', 1000);
            string language = "EN";
            string targetCurrencies = "EUR,GBP";

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("Invalid date format. Please use 'yyyy-MM-dd'.", errorResponse.Message);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsBadRequest_ForOverlyLongLanguage()
        {
            // Arrange
            string baseCurrency = "USD";
            string date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            string language = new string('E', 1000);
            string targetCurrencies = "EUR,GBP";

            // Act
            var result = await _controller.GetDailyExchangeRates(baseCurrency, date, language, targetCurrencies, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
            Assert.Equal("Invalid language value. Please use 'EN' or 'CZ'.", errorResponse.Message);
        }
    }
}
