using ExchangeRateUpdater.WebApi.Services;
using ExchangeRateUpdater.WebApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ExchangeRateUpdater.UnitTests.Services
{
    public class ExchangeRatesDownloaderUnitTests
    {
        private const string CorrectCnbExchangeRatesURLKey = "CnbExchangeRatesUrl";
        private const string CorrectCnbExchangeRatesURLValue = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string WrongCnbExchangeRatesURLKey = "WrongKey";
        private const string WrongCnbExchangeRatesURLValue = "WrongValue";
        private const string ExpectedRawCnbExchangeRates = "18 Apr 2023 #75\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|14.364\n" +
            "Brazil|real|1|BRL|4.330\nBulgaria|lev|1|BGN|11.952\nCanada|dollar|1|CAD|15.929\n";
        private const string ExpectedExceptionMessage = "Exchange rates source URL is missing in configuration";

        private Mock<IConfiguration> _configurationMock;
        private Mock<IConfigurationSection> _configurationSectionMock;
        private Mock<IExchangeRatesDownloaderFromURL> _exchangeRateDownloaderFromURL;

        private ExchangeRatesDownloader? _exchangeRatesDownloader;

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationSectionMock = new Mock<IConfigurationSection>();
            _exchangeRateDownloaderFromURL = new Mock<IExchangeRatesDownloaderFromURL>();

            _exchangeRateDownloaderFromURL.Setup(exchangeRateDownloaderFromURL => exchangeRateDownloaderFromURL
                                          .GetExchangeRatesRawTextFromURL(CorrectCnbExchangeRatesURLValue))
                                          .Returns(Task.FromResult(ExpectedRawCnbExchangeRates));
        }

        [Test]
        public async Task ExchangeRatesDownloader_ShouldReturnProperRawExchangeRates_WhenCorrectInputIsProvided()
        {
            SetConfigurationURLParameter(CorrectCnbExchangeRatesURLKey, CorrectCnbExchangeRatesURLValue);
            _exchangeRatesDownloader = new ExchangeRatesDownloader(_configurationMock.Object, _exchangeRateDownloaderFromURL.Object);

            var exchangesRates = await _exchangeRatesDownloader.GetRawExchangeRates();

            Assert.That(exchangesRates, Is.EqualTo(ExpectedRawCnbExchangeRates));
        }

        [Test]
        public void ExchangeRatesDownloader_ShouldThrowException_WhenConfigurationIsWrong()
        {
            SetConfigurationURLParameter(WrongCnbExchangeRatesURLKey, WrongCnbExchangeRatesURLValue);

            _exchangeRatesDownloader = new ExchangeRatesDownloader(_configurationMock.Object, _exchangeRateDownloaderFromURL.Object);

            Exception exception = Assert.ThrowsAsync<Exception>(async () => await _exchangeRatesDownloader.GetRawExchangeRates());
            Assert.That(exception.Message, Is.EqualTo(ExpectedExceptionMessage));
        }

        private void SetConfigurationURLParameter(string key, string value)
        {
            _configurationSectionMock.Setup(configurationSection => configurationSection.Value).Returns(value);
            _configurationSectionMock.Setup(configurationSection => configurationSection.Key).Returns(key);
            _configurationMock.Setup(configuration => configuration.GetSection(key)).Returns(_configurationSectionMock.Object);
        }
    }
}