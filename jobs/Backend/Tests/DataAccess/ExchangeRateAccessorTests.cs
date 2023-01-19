using DataAccess.Concrete;
using Microsoft.Extensions.Configuration;
using Moq;
using Tests.Helpers;

namespace Tests.DataAccess
{
    public class ExchangeRateAccessorTests
    {
        private Mock<IConfiguration> _configuration = new();
        public ExchangeRateAccessorTests()
        {
            var _configurationSection1 = new Mock<IConfigurationSection>();
            _configurationSection1.Setup(x => x.Key).Returns(FakeDataHelper.SourceCurrencyKey);
            _configurationSection1.Setup(x => x.Value).Returns(FakeDataHelper.SourceCurrencyKey);
            var _configurationSection2 = new Mock<IConfigurationSection>();
            _configurationSection2.Setup(x => x.Key).Returns(FakeDataHelper.SourceUrlKey);
            _configurationSection2.Setup(x => x.Value).Returns(FakeDataHelper.SourceUrlValue);        
            _configuration.Setup(c => c.GetSection(FakeDataHelper.SourceCurrencyKey)).Returns(_configurationSection1.Object);
            _configuration.Setup(c => c.GetSection(FakeDataHelper.SourceUrlKey)).Returns(_configurationSection2.Object);
        }

        [Fact]
        public async Task GetExchangeRates_Success_FromResource()
        {
            //arrange
            var _exchangeRateAccessor = new ExchangeRateAccessor(_configuration.Object);

            //act
            var _providerResponse =
                _exchangeRateAccessor.GetExchangeRates();

            //assert
            Assert.NotEmpty(_providerResponse);
            Assert.True(_providerResponse.Count() == 31);
        }
    }
}
