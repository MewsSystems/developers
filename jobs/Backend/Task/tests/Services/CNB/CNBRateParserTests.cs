using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.CNB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.Tests.Services.CNB
{
    public class CNBRateParserTests
    {
        private readonly Mock<ILogger<CNBRateParser>> _loggerMock;
        private readonly Mock<ICurrencyIsoService> _isoServiceMock;
        private readonly ExchangeRateOptions _options;
        private readonly HashSet<string> _validIsoCodes;

        public CNBRateParserTests()
        {
            _loggerMock = new Mock<ILogger<CNBRateParser>>();
            _isoServiceMock = new Mock<ICurrencyIsoService>();
            _options = TestFixtures.CreateValidOptions();
            _validIsoCodes = new HashSet<string> { "USD", "EUR", "JPY", "AUD", "BRL", "TST" };
            
            _isoServiceMock.Setup(x => x.GetValidIsoCodes())
                .Returns(_validIsoCodes);
        }

        [Fact]
        public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CNBRateParser(
                null,
                _isoServiceMock.Object,
                _loggerMock.Object));
        }

        [Fact]
        public void Constructor_WithNullIsoService_ShouldThrowArgumentNullException()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(_options);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CNBRateParser(
                optionsMock.Object,
                null,
                _loggerMock.Object));
        }

        [Fact]
        public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(_options);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CNBRateParser(
                optionsMock.Object,
                _isoServiceMock.Object,
                null));
        }

        [Fact]
        public void ParseRates_WithInvalidCurrencyCode_ShouldSkipInvalidCodes()
        {
            // Arrange
            var data = @"24 Jan 2025 #17
Country|Currency|Amount|Code|Rate
Australia|dollar|1|XXX|15.145
Brazil|real|1|BRL|3.924";

            var parser = CreateParser();

            // Act
            var rates = parser.ParseRates(data).ToList();

            // Assert
            Assert.Single(rates);
            Assert.Equal("BRL", rates[0].Code);
        }

        [Fact]
        public void ParseRates_WithLargeFile_ShouldParseAllRates()
        {
            // Arrange
            var data = File.ReadAllText("TestData/large-file.txt");
            var parser = CreateParser();

            // Act
            var rates = parser.ParseRates(data).ToList();

            // Assert
            Assert.Contains(rates, r => r.Code == "USD" && r.Rate == 24.398m);
            Assert.Contains(rates, r => r.Code == "EUR" && r.Rate == 25.175m);
            Assert.Contains(rates, r => r.Code == "JPY" && r.Amount == 100m);
        }

        [Fact]
        public void ParseRates_WithInvalidFormatFile_ShouldHandleErrors()
        {
            // Arrange
            var data = File.ReadAllText("TestData/invalid-format.txt");
            var parser = CreateParser();

            // Act
            var rates = parser.ParseRates(data).ToList();

            // Assert
            Assert.Single(rates); // Only EUR line is valid
            Assert.Equal("EUR", rates[0].Code);
            Assert.Equal(25.175m, rates[0].Rate);
        }

        [Fact]
        public void ParseRates_WithDuplicatesFile_ShouldKeepFirstOccurrenceOfEachCurrency()
        {
            // Arrange
            var data = File.ReadAllText("TestData/duplicates.txt");
            var parser = CreateParser();

            // Act
            var rates = parser.ParseRates(data).ToList();

            // Assert
            Assert.Equal(3, rates.Count); // Should have AUD, EUR, and USD (first occurrence of each)
            
            var audRate = rates.Single(r => r.Code == "AUD");
            Assert.Equal(1m, audRate.Amount);
            Assert.Equal(15.145m, audRate.Rate);

            var eurRate = rates.Single(r => r.Code == "EUR");
            Assert.Equal(1m, eurRate.Amount);
            Assert.Equal(25.175m, eurRate.Rate);

            var usdRate = rates.Single(r => r.Code == "USD");
            Assert.Equal(1m, usdRate.Amount);
            Assert.Equal(24.398m, usdRate.Rate);
        }

        [Fact]
        public void ParseRates_WithValidData_ShouldReturnParsedRates()
        {
            // Arrange
            var data = @"24 Jan 2025 #17
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.145
Brazil|real|1|BRL|3.924";

            var parser = CreateParser();

            // Act
            var rates = parser.ParseRates(data).ToList();

            // Assert
            Assert.Equal(2, rates.Count);
            
            var audRate = rates.First();
            Assert.Equal("AUD", audRate.Code);
            Assert.Equal(1m, audRate.Amount);
            Assert.Equal(15.145m, audRate.Rate);

            var brlRate = rates.Last();
            Assert.Equal("BRL", brlRate.Code);
            Assert.Equal(1m, brlRate.Amount);
            Assert.Equal(3.924m, brlRate.Rate);
        }

        [Fact]
        public void ParseRates_WithEmptyData_ShouldReturnEmptyCollection()
        {
            // Arrange
            var parser = CreateParser();

            // Act
            var rates = parser.ParseRates("").ToList();

            // Assert
            Assert.Empty(rates);
        }

        [Fact]
        public void ParseRates_WithInvalidFormat_ShouldSkipInvalidLines()
        {
            // Arrange
            var data = @"24 Jan 2025 #17
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.145
Invalid|Line|Without|Enough|Fields|Extra
Brazil|real|1|BRL|3.924";

            var parser = CreateParser();

            // Act
            var rates = parser.ParseRates(data).ToList();

            // Assert
            Assert.Equal(2, rates.Count);
            Assert.Contains(rates, r => r.Code == "AUD");
            Assert.Contains(rates, r => r.Code == "BRL");
        }

        [Fact]
        public void ParseRates_WithInvalidNumbers_ShouldSkipInvalidLines()
        {
            // Arrange
            var data = @"24 Jan 2025 #17
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.145
Japan|yen|invalid|JPY|15.539
Brazil|real|1|BRL|invalid";

            var parser = CreateParser();

            // Act
            var rates = parser.ParseRates(data).ToList();

            // Assert
            Assert.Single(rates);
            Assert.Equal("AUD", rates[0].Code);
        }

        [Theory]
        [InlineData("1234", 1234)]
        [InlineData("15.145", 15.145)]
        [InlineData("0.123", 0.123)]
        public void ParseRates_WithDifferentNumberFormats_ShouldParseCorrectly(string rateString, decimal expectedRate)
        {
            // Arrange
            var data = $@"24 Jan 2025 #17
Country|Currency|Amount|Code|Rate
Test|currency|1|TST|{rateString}";

            var parser = CreateParser();

            // Act
            var rates = parser.ParseRates(data).ToList();

            // Assert
            Assert.Single(rates);
            Assert.Equal(expectedRate, rates[0].Rate);
        }

        [Fact]
        public void ParseRates_WithOnlyHeaderAndDate_ShouldReturnEmptyCollection()
        {
            // Arrange
            var data = @"24 Jan 2025 #17
Country|Currency|Amount|Code|Rate";

            var parser = CreateParser();

            // Act
            var rates = parser.ParseRates(data).ToList();

            // Assert
            Assert.Empty(rates);
        }

        private CNBRateParser CreateParser()
        {
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(_options);

            return new CNBRateParser(
                optionsMock.Object,
                _isoServiceMock.Object,
                _loggerMock.Object);
        }
    }
} 