using ExchangeRateUpdater.BusinessLogic.Implementations;
using ExchangeRateUpdater.BusinessLogic.Interfaces;
using ExchangeRateUpdater.BusinessLogic.Models;
using Moq;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public void WhenGetExchangeRates_WithAnyValue_ShouldBeOk()
        {
            var expectedResult = new List<ExchangeRate>();
            var mockedFactory = new Mock<IExchangesServicesFactory>();
            var mockedService = new Mock<IExchangeService>();
            mockedFactory.Setup(x => x.GetExchangeService(It.IsAny<Currency>())).Returns(mockedService.Object);
            mockedService.Setup(x => x.GetExchangeRates(It.IsAny<IEnumerable<Currency>>())).Returns(expectedResult);

            var rates = new ExchangeRateProvider(mockedFactory.Object).GetExchangeRates(null, null);

            Assert.Equal(expectedResult, rates);
            mockedFactory.Verify(x => x.GetExchangeService(It.IsAny<Currency>()), Times.Once());
            mockedService.Verify(x => x.GetExchangeRates(It.IsAny<IEnumerable<Currency>>()), Times.Once());
        }
    }
}