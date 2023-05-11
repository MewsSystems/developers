using ExchangeRateUpdater.DAL.Implementations;
using ExchangeRateUpdater.DAL.Models;
using ExchangeRateUpdater.Tests.Utility;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace ExchangeRateUpdater.Tests.DALTests
{
    public class DataScrapperShould
    {
        private readonly DataScrapper _dataScrapper;
        public DataScrapperShould() 
        {
            var _logger = new Mock<ILogger<DataScrapper>>();
            _dataScrapper = new DataScrapper(_logger.Object);
        }
        [Fact]
        public void should_return_rates_on_website()
        {
            var CNBUrl = TestParameters.GetCNBWebsite();
            var result = _dataScrapper.GetRatesFromWeb(CNBUrl);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<RateModel>>(result);
        }
        [Fact]
        public void should_return_web_exception_if_url_is_wrong()
        {
            var CNBUrl = TestParameters.GetWrongCNBWebsiteURL();

            Assert.Throws<WebException>(() => _dataScrapper.GetRatesFromWeb(CNBUrl));

        }
    }
}
