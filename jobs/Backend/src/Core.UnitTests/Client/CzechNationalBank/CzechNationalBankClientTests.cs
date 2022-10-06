using Common.Configuration;
using Core.Client.CzechNationalBank;
using Core.Models;
using Core.Parser;
using ExchangeRateUpdater.Common.Http;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Core.UnitTests.Client.CzechNationalBank
{
    public class CzechNationalBankClientTests
    {
        private readonly Mock<ILogger<CzechNationalBankClient>> _mockLogger;
        private readonly Mock<IConfigurationWrapper> _mockConfigurationWrapper;
        private readonly Mock<IHttpWrapper> _mockHttpWrapper;
        private readonly Mock<IResponseParser> _mockResponseParser;
        private readonly CzechNationalBankClient _objectToTest;

        private string testClientData = "04 Oct 2022 #192\n\rCountry|Currency|Amount|Code|Rate\n\rAustralia|dollar|1|AUD|16.027\n\rBrazil|real|1|BRL|4.854";

        public CzechNationalBankClientTests()
        {
            _mockLogger = new Mock<ILogger<CzechNationalBankClient>>();
            _mockConfigurationWrapper = new Mock<IConfigurationWrapper>();
            _mockHttpWrapper = new Mock<IHttpWrapper>();
            _mockResponseParser = new Mock<IResponseParser>();

            _objectToTest = new CzechNationalBankClient(_mockLogger.Object, _mockConfigurationWrapper.Object, _mockHttpWrapper.Object, _mockResponseParser.Object);
        }

        public class GetExhcnageRatesTests : CzechNationalBankClientTests
        {
            private void SetupGetExhcnageRatesTests()
            {
                _mockConfigurationWrapper
                    .Setup(x => x.GetConfigValueAsString(It.IsAny<string>(), false))
                    .Returns("");

                _mockHttpWrapper
                    .Setup(x => x.HttpGet(It.IsAny<string>(), null))
                    .ReturnsAsync(testClientData);

                _mockResponseParser
                    .Setup(x => x.ParseResponse(testClientData))
                    .Returns(
                        new List<ExchangeRate>()
                        {
                            new ExchangeRate(
                                new Currency("AUD"),
                                new Currency("CZK"),
                                (decimal) 16.027
                                )
                        }
                    );
            }

            [Fact]
            public async Task Gets_RateData_From_Client_Source()
            {
                // Arrange
                SetupGetExhcnageRatesTests();

                // Act
                await _objectToTest.GetExchangeRates();

                // Assert
                _mockHttpWrapper.Verify(x => x.HttpGet(It.IsAny<string>(), null), Times.Once);
            }

            [Fact]
            public async Task Returns_ParsedRateDate_List_Retrieved_From_Client()
            {
                // Arrange
                SetupGetExhcnageRatesTests();

                // Act
                var actual = await _objectToTest.GetExchangeRates();

                // Assert
                _mockResponseParser.Verify(x => x.ParseResponse(testClientData), Times.Once);
                actual.Should().HaveCountGreaterThan(0);
            }
        }
    }
}
