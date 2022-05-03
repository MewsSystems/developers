using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Integration;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.UnitTests
{
    public class CnbExchangeRateRepositoryTests
    {
        [Fact]
        public async Task Test()
        {
            // Arrange
            Mock<ICnbApiClient> apiClientMock = new Mock<ICnbApiClient>();
            apiClientMock
                .Setup(x => x.GetBasicRatesAsync())
                .ReturnsAsync(new[] {
                    new CnbExchangeRate()
                    {
                        CountryName = "Bidenuv dolar",
                        CurrencyName = "dolar",
                        Count = 1,
                        CurrencyCode = "USD",
                        Rate = 23.2M
                    }
                });

            CnbExchangeRateRepository SUT = new CnbExchangeRateRepository(apiClientMock.Object);

            // Act
            await SUT.Initialize();
            bool hasData = SUT.Any();

            // Assert
            Assert.True(hasData);
        }
    }
}