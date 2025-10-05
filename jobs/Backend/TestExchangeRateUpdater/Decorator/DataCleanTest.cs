using System.Threading.Tasks;
using ExchangeRateUpdater.Decorator;
using Moq;
using Xunit;

namespace TestExchangeRateUpdater.Decorator
{
    public class DataCleanTest
    {
        [Fact]
        public async Task Load_FiltersAndPassesCorrectLinesToWrapper()
        {
            // Arrange
            string input = "United States|Dollar|1|USD|25.50\n" +
                           "InvalidLine\n" +
                           "Germany|Euro|100|EUR|27.30\n" +
                           "AnotherInvalid | Line\n" +
                           "United Kingdom|Pound|1000|GBP|30.00";

            string expected = "United States|Dollar|1|USD|25.50\r\n" +
                              "Germany|Euro|100|EUR|27.30\r\n" +
                              "United Kingdom|Pound|1000|GBP|30.00\r\n";

            Mock<ILoadRates> wrapperMock = new();
            wrapperMock.Setup(w => w.Load(expected)).ReturnsAsync(true);

            DataClean dataClean = new(wrapperMock.Object);

            // Act
            var result = await dataClean.Load(input);

            // Assert
            Assert.True(result);
            wrapperMock.Verify(w => w.Load(expected), Times.Once);
        }
    }
}