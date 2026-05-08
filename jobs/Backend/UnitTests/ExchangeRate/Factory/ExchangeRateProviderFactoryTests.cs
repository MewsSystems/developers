using ExchangeRateUpdater.ExchangeRate.Exception;
using ExchangeRateUpdater.ExchangeRate.Factory;
using ExchangeRateUpdater.ExchangeRate.Model;
using ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderFactoryTests
    {
        private readonly ExchangeRateProviderFactory _factory;
        private readonly Mock<ICzechNationalBankClient> _mockCzechNationalBankClient;
        private readonly Mock<ILogger<CzechNationalBankExchangeRateProvider>> _mockCzechNationalBankExchangeRateProviderLogger;
        private readonly Mock<IOptionsMonitor<DefaultExchangeRateProviderConfig>> _mockDefaultExchangeRateProviderMonitor;

        public ExchangeRateProviderFactoryTests()
        {
            _mockCzechNationalBankClient = new Mock<ICzechNationalBankClient>();
            _mockCzechNationalBankExchangeRateProviderLogger = new Mock<ILogger<CzechNationalBankExchangeRateProvider>>();
            _mockDefaultExchangeRateProviderMonitor = new Mock<IOptionsMonitor<DefaultExchangeRateProviderConfig>>();

            _factory = new ExchangeRateProviderFactory(
                _mockDefaultExchangeRateProviderMonitor.Object,
                _mockCzechNationalBankClient.Object,
                _mockCzechNationalBankExchangeRateProviderLogger.Object);
        }

        [Fact]
        public void GetProvider_Returns_CzechNationalBankExchangeRateProvider_For_CZK()
        {
            // Arrange
            var defaultExchangeRateProviderConfigMock = new Mock<IOptionsMonitor<DefaultExchangeRateProviderConfig>>();
            var czechNationalBankClientMock = new Mock<ICzechNationalBankClient>();
            var czechNationalBankExchangeRateProviderLoggerMock = new Mock<ILogger<CzechNationalBankExchangeRateProvider>>();

            var defaultExchangeRateProviderConfig = new DefaultExchangeRateProviderConfig
            {
                ["CZK"] = "CzechNationalBank"
            };

            defaultExchangeRateProviderConfigMock.Setup(m => m.CurrentValue).Returns(defaultExchangeRateProviderConfig);

            var factory = new ExchangeRateProviderFactory(
                defaultExchangeRateProviderConfigMock.Object,
                czechNationalBankClientMock.Object,
                czechNationalBankExchangeRateProviderLoggerMock.Object);

            var currency = new Currency("CZK");

            // Act
            var provider = factory.GetProvider(currency);

            // Assert
            Assert.NotNull(provider);
            Assert.IsType<CzechNationalBankExchangeRateProvider>(provider);
        }

        [Fact]
        public void GetProvider_Throws_Exception_For_Unsupported_Currency()
        {
            // Arrange
            var defaultExchangeRateProviderConfigMock = new Mock<IOptionsMonitor<DefaultExchangeRateProviderConfig>>();
            var czechNationalBankClientMock = new Mock<ICzechNationalBankClient>();
            var czechNationalBankExchangeRateProviderLoggerMock = new Mock<ILogger<CzechNationalBankExchangeRateProvider>>();

            var factory = new ExchangeRateProviderFactory(
                defaultExchangeRateProviderConfigMock.Object,
                czechNationalBankClientMock.Object,
                czechNationalBankExchangeRateProviderLoggerMock.Object);

            // Act and Assert
            var unsupportedCurrency = new Currency("USC"); // USC is not supported in the current setup
            Assert.Throws<ExchangeRateUpdaterException>(() => factory.GetProvider(unsupportedCurrency));
        }

        [Fact]
        public void GetProvider_ThrowsException_WhenDefaultProviderNotDefined()
        {
            // Arrange
            var defaultExchangeRateProviderMonitorMock = new Mock<IOptionsMonitor<DefaultExchangeRateProviderConfig>>();
            defaultExchangeRateProviderMonitorMock.Setup(x => x.CurrentValue).Returns([]);

            var czechNationalBankClientMock = new Mock<ICzechNationalBankClient>();
            var czechNationalBankExchangeRateProviderLoggerMock = new Mock<ILogger<CzechNationalBankExchangeRateProvider>>();

            var factory = new ExchangeRateProviderFactory(
                defaultExchangeRateProviderMonitorMock.Object,
                czechNationalBankClientMock.Object,
                czechNationalBankExchangeRateProviderLoggerMock.Object);

            var currency = new Currency("CZK");

            // Act & Assert
            Assert.Throws<ExchangeRateUpdaterException>(() => factory.GetProvider(currency));
        }
    }
}
