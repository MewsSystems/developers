using Common.Configuration;
using Core.Client.Provider;
using Core.Models;
using Core.Parser;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Common.Http;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Core.UnitTests.Provider
{
    public class ExchangeRateProviderTests
    {
        private readonly ExchangeRateProvider _objectToTest;
        private readonly Mock<ILogger<ExchangeRateProvider>> _mockLogger;
        private readonly Mock<IClient> _mockClient;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConfigurationWrapper> _mockConfigurationWrapper;
        private readonly Mock<IHttpWrapper> _mockHttpWrapper;
        private readonly Mock<IResponseParser> _mockResponseParser;

        private string testCsvData = "04 Oct 2022 #192\n\rCountry|Currency|Amount|Code|Rate\n\rAustralia|dollar|1|AUD|16.027\n\rBrazil|real|1|BRL|4.854";
        private IEnumerable<ExchangeRate> testEcchangeRateData 
                        = new List<ExchangeRate>()
                        {
                            new ExchangeRate(
                                new Currency("AUD"),
                                new Currency("CZK"),
                                (decimal) 16.027
                                )
                        };

        public ExchangeRateProviderTests()
        {
            _mockLogger = new Mock<ILogger<ExchangeRateProvider>>();
            _mockClient = new Mock<IClient>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfigurationWrapper = new Mock<IConfigurationWrapper>();
            _mockHttpWrapper = new Mock<IHttpWrapper>();
            _mockResponseParser = new Mock<IResponseParser>();
            _objectToTest = new ExchangeRateProvider(_mockLogger.Object, _mockConfiguration.Object, _mockClient.Object);
        }
        public class GetExhcnageRatesTests : ExchangeRateProviderTests
        {
            private void SetupGetExchangeRatesTests()
            {
                _mockConfigurationWrapper
                    .Setup(x => x.GetConfigValueAsString(It.IsAny<string>(), false))
                    .Returns("");

                _mockHttpWrapper
                    .Setup(x => x.HttpGet(It.IsAny<string>(), null))
                    .ReturnsAsync(testCsvData);

                _mockResponseParser
                    .Setup(x => x.ParseResponse(testCsvData))
                    .Returns(testEcchangeRateData);

                _mockClient
                    .Setup(x => x.GetExchangeRates())
                    .ReturnsAsync(testEcchangeRateData);
            }

            [Fact]
            public async Task Gets_Filtered_RateData()
            {
                // Arrange                
                SetupGetExchangeRatesTests();
                var currencies = new List<Currency> { new Currency("AUD") };

                // Act
                var actual = await _objectToTest.GetExchangeRates(currencies);

                // Assert
                actual.Should().Contain(rate => currencies.Any(currency => currency.Code.Equals(rate.SourceCurrency.Code)));
                actual.Should().NotContain(rate => rate.SourceCurrency.Code.Equals("USD"));
            }
        }
    }
}
