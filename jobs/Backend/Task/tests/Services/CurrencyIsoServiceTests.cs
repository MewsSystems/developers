using System;
using System.IO;
using System.Linq;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.Tests.Services
{
    public class CurrencyIsoServiceTests
    {
        private readonly Mock<ILogger<CurrencyIsoService>> _loggerMock;
        private readonly string _testDataPath;

        public CurrencyIsoServiceTests()
        {
            _loggerMock = new Mock<ILogger<CurrencyIsoService>>();
            _testDataPath = Path.Combine("..", "data", "currency-codes-test.json");
            
            // Create test data file if it doesn't exist
            if (!File.Exists(_testDataPath))
            {
                var testData = @"[
                    {
                        ""entity"": ""TEST COUNTRY 1"",
                        ""currency"": ""Test Currency 1"",
                        ""alphabeticcode"": ""TST"",
                        ""numericcode"": ""001"",
                        ""minorunit"": ""2"",
                        ""withdrawaldate"": null
                    },
                    {
                        ""entity"": ""TEST COUNTRY 2"",
                        ""currency"": ""Test Currency 2"",
                        ""alphabeticcode"": ""TS2"",
                        ""numericcode"": ""002"",
                        ""minorunit"": ""2"",
                        ""withdrawaldate"": ""2023-01""
                    },
                    {
                        ""entity"": ""TEST COUNTRY 3"",
                        ""currency"": ""Test Currency 3"",
                        ""alphabeticcode"": ""TS3"",
                        ""numericcode"": ""003"",
                        ""minorunit"": ""2"",
                        ""withdrawaldate"": null
                    }
                ]";
                Directory.CreateDirectory(Path.Combine("..", "data"));
                File.WriteAllText(_testDataPath, testData);
            }
        }

        [Fact]
        public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CurrencyIsoService(null, _loggerMock.Object));
        }

        [Fact]
        public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
        {
            // Arrange
            var optionsMock = CreateOptionsMock();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CurrencyIsoService(optionsMock.Object, null));
        }

        [Fact]
        public void Constructor_WithInvalidFilePath_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<CurrencyOptions>>();
            optionsMock.Setup(x => x.Value).Returns(new CurrencyOptions { IsoCodesFilePath = "nonexistent.json" });

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new CurrencyIsoService(optionsMock.Object, _loggerMock.Object));
            Assert.Equal("Failed to load currency codes from nonexistent.json", exception.Message);
        }

        [Fact]
        public void GetValidIsoCodes_ShouldReturnOnlyActiveIsoCodes()
        {
            // Arrange
            var service = CreateService();

            // Act
            var codes = service.GetValidIsoCodes();

            // Assert
            Assert.Equal(2, codes.Count);
            Assert.Contains("TST", codes);
            Assert.Contains("TS3", codes);
            Assert.DoesNotContain("TS2", codes); 
        }

        private Mock<IOptions<CurrencyOptions>> CreateOptionsMock()
        {
            var optionsMock = new Mock<IOptions<CurrencyOptions>>();
            optionsMock.Setup(x => x.Value).Returns(new CurrencyOptions { IsoCodesFilePath = _testDataPath });
            return optionsMock;
        }

        private CurrencyIsoService CreateService()
        {
            return new CurrencyIsoService(CreateOptionsMock().Object, _loggerMock.Object);
        }
    }
} 