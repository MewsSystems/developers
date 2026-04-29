using ExchangeRateAPI.Controllers;
using ExchangeRateAPI.Models;
using ExchangeRateUpdater.Business;
using Moq;

namespace ExchangeRateAPIUnitTest
{
    public class ExchangeRateControllerTest
    {
        private readonly ExchangeRateController __ExchangeRateController;
        private readonly Mock<IExchangeRateProvider> __ExchangeRateProviderMock;

        public ExchangeRateControllerTest()
        {
            __ExchangeRateProviderMock = new Mock<IExchangeRateProvider>();
            __ExchangeRateController = new ExchangeRateController(__ExchangeRateProviderMock.Object);
        }

        [Fact]
        public async Task ExchangeRateControllerTest_GetAsync_ShouldReturnResultsAsync()
        {
            List<ExchangeRate> _ExchangeRateMock =
            [
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 20.43m),
                new ExchangeRate(new Currency("AUD"), new Currency("CZK"), 14.48m),
            ];

            __ExchangeRateProviderMock
                .Setup(mock => mock.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>()))
                .Returns(Task.FromResult(_ExchangeRateMock));

            List<ExchangeRateResponse> _Response = await __ExchangeRateController.GetAsync();

            Assert.NotNull(_Response);
            Assert.Equal(2, _Response.Count);
        }
    }
}
