using ExchangeRateUpdater.Domain.Model;
using ExchangeRateUpdater.Implementation.Services;
using ExchangeRateUpdater.Interface.Configuration;
using ExchangeRateUpdater.Interface.Services;
using ExchangeRateUpdater.Test.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Test.Services
{
    [TestClass]
    public class CnbApiServiceTest
    {
        private readonly Mock<ILogger<ICnbApiService>> _loggerMock;
        private readonly Mock<IOptions<CnbSettings>> _cnbSettingsMock;

        public CnbApiServiceTest()
        {
            _loggerMock = new Mock<ILogger<ICnbApiService>>();
            _cnbSettingsMock = new Mock<IOptions<CnbSettings>>();
        }

        [TestMethod]
        public async Task GetExchangeRates()
        {
            _cnbSettingsMock.SetupGet(x => x.Value).Returns(TestObjects.CnbSettings);            

            var service = new CnbApiService(_loggerMock.Object, _cnbSettingsMock.Object);

            var response = await service.GetExchangeRates();

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(IEnumerable<ExchangeRateEntity>));
        }
    }
}
