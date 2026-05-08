using ExchangeRateUpdater.Errors;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Builders;
using ExchangeRateUpdater.Services.Clients;
using ExchangeRateUpdater.Services.Handlers;
using ExchangeRateUpdater.Services.Parsers;
using FluentAssertions;
using FluentResults;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<ICnbApiClient> _apiClientMock;
        private readonly Mock<ICnbDataParser> _parserMock;
        private readonly Mock<IExchangeRateBuilder> _builderMock;
        private readonly Mock<ILogger<ExchangeRateProvider>> _loggerMock;
        private readonly ExchangeRateProvider _provider;

        public ExchangeRateProviderTests()
        {
            _apiClientMock = new Mock<ICnbApiClient>();
            _parserMock = new Mock<ICnbDataParser>();
            _builderMock = new Mock<IExchangeRateBuilder>();
            _loggerMock = new Mock<ILogger<ExchangeRateProvider>>();

            _provider = new ExchangeRateProvider(
                _apiClientMock.Object,
                _parserMock.Object,
                _builderMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public void GetExchangeRates_WithValidData_ReturnsExchangeRates()
        {
            // Arrange
            var currencies = new[] { new Currency("USD"), new Currency("EUR") };
            var apiData = "sample CNB data";
            var parsedData = new CnbExchangeRateData
            {
                Date = DateTime.Now,
                Rates = new List<CnbExchangeRateEntry>
                {
                    new() { Code = "USD", Rate = 20.5m, Amount = 1 },
                    new() { Code = "EUR", Rate = 24.5m, Amount = 1 }
                }
            };
            var expectedRates = new[]
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 20.5m),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.5m)
            };

            _apiClientMock.Setup(x => x.GetExchangeRateDataAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(apiData));

            _parserMock.Setup(x => x.Parse(apiData))
                .Returns(Result.Ok(parsedData));

            _builderMock.Setup(x => x.BuildExchangeRates(currencies, parsedData))
                .Returns(expectedRates);

            // Act
            var result = _provider.GetExchangeRates(currencies);

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedRates);

            _apiClientMock.Verify(x => x.GetExchangeRateDataAsync(It.IsAny<CancellationToken>()), Times.Once);
            _parserMock.Verify(x => x.Parse(apiData), Times.Once);
            _builderMock.Verify(x => x.BuildExchangeRates(currencies, parsedData), Times.Once);
        }

        [Fact]
        public void GetExchangeRates_WhenBuilderThrowsException_ThrowsCnbExceptionWithUnexpectedError()
        {
            // Arrange
            var currencies = new[] { new Currency("USD") };
            var apiData = "sample data";
            var parsedData = new CnbExchangeRateData();

            _apiClientMock.Setup(x => x.GetExchangeRateDataAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(apiData));

            _parserMock.Setup(x => x.Parse(apiData))
                .Returns(Result.Ok(parsedData));

            _builderMock.Setup(x => x.BuildExchangeRates(currencies, parsedData))
                .Throws(new InvalidOperationException("Builder error"));

            // Act & Assert
            var exception = Assert.Throws<CnbException>(() => _provider.GetExchangeRates(currencies));
            exception.ErrorCode.Should().Be(CnbErrorCode.UnexpectedError);
            exception.InnerException.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public void GetExchangeRates_WithNullCurrencies_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _provider.GetExchangeRates(null));
            exception.ParamName.Should().Be("currencies");
        }

        [Fact]
        public void GetExchangeRates_WithEmptyCurrencies_ReturnsEmptyCollection()
        {
            // Arrange
            var currencies = Enumerable.Empty<Currency>();

            // Act
            var result = _provider.GetExchangeRates(currencies);

            // Assert
            result.Should().BeEmpty();
            _apiClientMock.Verify(x => x.GetExchangeRateDataAsync(It.IsAny<CancellationToken>()), Times.Never);
            _parserMock.Verify(x => x.Parse(It.IsAny<string>()), Times.Never);
            _builderMock.Verify(x => x.BuildExchangeRates(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CnbExchangeRateData>()), Times.Never);
        }

        [Fact]
        public void GetExchangeRates_WhenApiClientFails_ThrowsCnbException()
        {
            // Arrange
            var currencies = new[] { new Currency("USD") };
            var apiError = ErrorHandler.Handle<string>(CnbErrorCode.NetworkError, "Network connection failed");

            _apiClientMock.Setup(x => x.GetExchangeRateDataAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiError);

            // Act & Assert
            var exception = Assert.Throws<CnbException>(() => _provider.GetExchangeRates(currencies));
            exception.ErrorCode.Should().Be(CnbErrorCode.NetworkError);
            exception.Message.Should().Be("Network connection failed");

            _parserMock.Verify(x => x.Parse(It.IsAny<string>()), Times.Never);
            _builderMock.Verify(x => x.BuildExchangeRates(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CnbExchangeRateData>()), Times.Never);
        }

        [Fact]
        public void GetExchangeRates_WhenParserFails_ThrowsCnbException()
        {
            // Arrange
            var currencies = new[] { new Currency("EUR") };
            var apiData = "invalid data format";
            var parseError = ErrorHandler.Handle<CnbExchangeRateData>(CnbErrorCode.InvalidDateFormat, "Unable to parse date");

            _apiClientMock.Setup(x => x.GetExchangeRateDataAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(apiData));

            _parserMock.Setup(x => x.Parse(apiData))
                .Returns(parseError);

            // Act & Assert
            var exception = Assert.Throws<CnbException>(() => _provider.GetExchangeRates(currencies));
            exception.ErrorCode.Should().Be(CnbErrorCode.InvalidDateFormat);
            exception.Message.Should().Be("Unable to parse date");

            _builderMock.Verify(x => x.BuildExchangeRates(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CnbExchangeRateData>()), Times.Never);
        }
    }
}
