using ExchangeRateUpdater.WebApi.Services.ExchangeRateDownloader;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ExchangeRateUpdater.WebApi.Tests
{
    
    [TestClass]
    public class ExchangeRateDownloaderTests
    {
        private const string ExchangeRatesSource = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string ExchangeRatesSourceKeyName = "ExchangeRatesSource";

        private Mock<IConfiguration> ConfigurationMock { get; } = new();
        
        public ExchangeRateDownloaderTests()
        {
            var exchangeRatesSourceSection = new Mock<IConfigurationSection>();
            exchangeRatesSourceSection.Setup(x => x.Key).Returns(ExchangeRatesSourceKeyName);
            exchangeRatesSourceSection.Setup(x => x.Value).Returns(ExchangeRatesSource);

            ConfigurationMock.Setup(c => c.GetSection(ExchangeRatesSourceKeyName)).Returns(exchangeRatesSourceSection.Object);

        }

        [TestMethod]
        public async Task OnlineSourceDownload_Returns_Values()
        {
            //arrange
            var exchangeRateDownloader = new ExchangeRateDownloader(ConfigurationMock.Object);

            //act
            string exchangeRatesDownloaded = await exchangeRateDownloader.DownloadExchangeRates();

            //assert
            Assert.IsFalse(string.IsNullOrEmpty(exchangeRatesDownloaded));
        }

    }
}
