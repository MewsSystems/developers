using ExchangeRateProvider.Cache;
using Model.Entities;
using NSubstitute;

namespace ExchangeRateProvider.Test
{
    [TestClass]
    public class ExchangeRateServiceTests
    {
        private readonly ICache<Currency, ExchangeRate> exchangeRateService = Substitute.For<ICache<Currency, ExchangeRate>>();

        [TestMethod]
        public void GetExchangeRates_Should_ReturnEmptyWhen()
        {
        }
    }
}