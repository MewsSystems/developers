using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Domain;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.UnitTests
{
    public class CnbExchangeRateProviderTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            Mock<IExchangeRateRepository> repositoryMock = new Mock<IExchangeRateRepository>();
            repositoryMock.Setup(x=> x.Any()).Returns(true);
            repositoryMock
                .Setup(x => x.TryGet(It.IsAny<string>()))
                .Returns(new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.2M));
                
            CnbExchangeRateProvider SUT = new CnbExchangeRateProvider(repositoryMock.Object);

            // Act
            var result = SUT.GetExchangeRates(new[] { new Currency("USD") });

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}