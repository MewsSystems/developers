using System.Reflection;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Chain_of_Responsibility;
using ExchangeRateUpdater.Decorator;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Singleton;
using Moq;
using Xunit;

namespace TestExchangeRateUpdater.Chain_of_Responsibility
{
    public class CNBTest
    {
        [Fact]
        public void GetExchangeRate_Return_null()
        {
            // Arrange
            var db = DB.GetInstance();
            // Use reflection to clear the DB singleton for test isolation
            var ratesField = typeof(DB).GetField("_rates", BindingFlags.NonPublic | BindingFlags.Instance);
            ratesField.SetValue(db, new System.Collections.Generic.Dictionary<string, Rate>());

            Mock<LoadRates> wrapperMock = new (MockBehavior.Default, new Mock<ILoadRates>().Object);
            wrapperMock.Setup(w => w.Load(It.IsAny<string>())).ReturnsAsync(true);

            CNB cnb = new(wrapperMock.Object);

            Currency currency = new("XYZ");

            // Act
            ExchangeRate result = cnb.GetExchangeRate(currency);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetExchangeRate_ReturnsRate_WhenRateExistsInMockedDB()
        {
            // Arrange
            var db = DB.GetInstance();
            // Use reflection to clear the DB singleton for test isolation
            var ratesField = typeof(DB).GetField("_rates", BindingFlags.NonPublic | BindingFlags.Instance);
            ratesField.SetValue(db, new System.Collections.Generic.Dictionary<string, Rate>());

            var currency = new Currency("USD");
            var expectedRate = new Rate("United States", "Dollar", 1, "USD", 25.50m);
            db.Add("USD", expectedRate);

            Mock<LoadRates> wrapperMock = new(MockBehavior.Default, new Mock<ILoadRates>().Object);
            wrapperMock.Setup(w => w.Load(It.IsAny<string>())).ReturnsAsync(true);

            var cnb = new CNB(wrapperMock.Object);

            // Act
            var result = cnb.GetExchangeRate(currency);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(currency.Code, result.SourceCurrency.Code);
            Assert.Equal("CZK", result.TargetCurrency.Code);
            Assert.Equal(25.50m, result.Value);
        }
    }
}