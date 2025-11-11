using ExchangeRateUpdater.Infrastructure.CNB;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdaterTests.Infrastructure.CNB
{
    public class CNBExchangeRateParserTests
    {
        private readonly Mock<ILogger<CNBExchangeRateParser>> mockLogger = new();
        private readonly CNBExchangeRateParser sut;

        public CNBExchangeRateParserTests()
        {
            sut = new CNBExchangeRateParser(mockLogger.Object);
        }

        [Fact]
        public void GivenValidCNBData_WhenParse_ThenReturnsRatesAndMetadata()
        {
            // Arrange
            var cnbData = "16 May 2025 #93\r\nCountry|Currency|Amount|Code|Rate\r\nUSA|dollar|1|USD|22.222\r\nEMU|euro|1|EUR|24.444";

            // Act
            var (metadata, records) = sut.Parse(cnbData);

            // Assert
            metadata.DateTime.Should().Be(new DateTime(2025, 5, 16));
            metadata.Identifier.Should().Be(93);
            records.Should().HaveCount(2);
            records.Should().Contain(r => r.TargetCurrency.Code == "USD" && r.Value == 22.222m);
            records.Should().Contain(r => r.TargetCurrency.Code == "EUR" && r.Value == 24.444m);
        }

        [Fact]
        public void GivenEmptyData_WhenParse_ThenReturnsEmpty()
        {
            // Act
            var (metadata, records) = sut.Parse("");

            // Assert
            records.Should().BeEmpty();
            metadata.DateTime.Should().Be(DateTime.MinValue);
        }

        [Fact]
        public void GivenMalformedHeader_WhenParse_ThenReturnsEmpty()
        {
            // Arrange
            var cnbData = "not a date\r\nCountry|Currency|Amount|Code|Rate";

            // Act
            var (metadata, records) = sut.Parse(cnbData);

            // Assert
            records.Should().BeEmpty();
            metadata.DateTime.Should().Be(DateTime.MinValue);
        }
    }
}
