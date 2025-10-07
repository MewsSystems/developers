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

        [Theory]
        [InlineData("")]
        [InlineData("fail1|fail1|fail1|fail1")]
        [InlineData("fail2|fail2|fail2|fail2|2")]
        [InlineData("fail3|fail3|3|fail3|fail3")]
        [InlineData("fail4|fail4|fail4|fail4|fail4")]
        public async Task Load_No_data(string input)
        {
            // Assert
            Mock<ILoadRates> wrapperMock = new();

            DataClean dataClean = new(wrapperMock.Object);

            //Act & Assert
            await Assert.ThrowsAsync<Exception>(() => dataClean.Load(input));
        }

        [Fact]
        public async Task Load_Throw_ArgumentNullException()
        {
            // Assert
            Mock<ILoadRates> wrapperMock = new();

            DataClean dataClean = new(wrapperMock.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => dataClean.Load(null));
        }
    }
}