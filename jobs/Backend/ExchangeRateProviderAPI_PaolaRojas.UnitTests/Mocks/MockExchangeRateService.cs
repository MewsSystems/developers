using ExchangeRateProviderAPI_PaolaRojas.Models;
using ExchangeRateProviderAPI_PaolaRojas.Models.Responses;
using ExchangeRateProviderAPI_PaolaRojas.Services;
using Moq;

namespace ExchangeRateProviderAPI_PaolaRojas.UnitTests.Mocks
{
    public static class MockExchangeRateService
    {
        public static Mock<IExchangeRateService> WithResult(ExchangeRateResponse? response)
        {
            var mock = new Mock<IExchangeRateService>();
            mock.Setup(s => s.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>()))
                .ReturnsAsync(response);
            return mock;
        }
    }
}
