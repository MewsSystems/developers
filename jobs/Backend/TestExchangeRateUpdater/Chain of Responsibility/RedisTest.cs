using ExchangeRateUpdater.Chain_of_Responsibility;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Singleton;
using ExchangeRateUpdater;

namespace TestExchangeRateUpdater.Chain_of_Responsibility
{
    public class RedisTest
    {
        [Fact]
        public void GetExchangeRate_ReturnsExchangeRate_WhenRateExists()
        {
            // Arrange
            var db = DB.GetInstance();
            // Clear DB for test isolation
            var ratesField = typeof(DB).GetField("_rates", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            ratesField.SetValue(db, new Dictionary<string, Rate>());

            var currency = new Currency("USD");
            var expectedRate = new Rate("United States", "Dollar", 1, "USD", 25.50m);
            db.Add("USD", expectedRate);

            var redis = new Redis();

            // Act
            var result = redis.GetExchangeRate(currency);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(currency.Code, result.SourceCurrency.Code);
            Assert.Equal("CZK", result.TargetCurrency.Code);
            Assert.Equal(25.50m, result.Value);
        }

        [Fact]
        public void GetExchangeRate_ReturnsNull_WhenRateDoesNotExistAndDbIsNotEmpty()
        {
            // Arrange
            var db = DB.GetInstance();
            // Clear DB for test isolation
            var ratesField = typeof(DB).GetField("_rates", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            ratesField.SetValue(db, new Dictionary<string, Rate>());

            // Add a different rate to make DB not empty
            db.Add("EUR", new Rate("Germany", "Euro", 1, "EUR", 27.00m));

            var redis = new Redis();
            var currency = new Currency("USD");

            // Act
            var result = redis.GetExchangeRate(currency);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetExchangeRate_CallsNextHandler_WhenDbIsEmpty()
        {
            // Arrange
            var db = DB.GetInstance();
            // Clear DB for test isolation
            var ratesField = typeof(DB).GetField("_rates", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            ratesField.SetValue(db, new Dictionary<string, Rate>());

            var redis = new Redis();
            var currency = new Currency("USD");

            // Set up a mock next handler
            var nextHandler = new TestHandler();
            redis.SetNext(nextHandler);

            // Act
            var result = redis.GetExchangeRate(currency);

            // Assert
            Assert.True(nextHandler.WasCalled);
            Assert.Equal(nextHandler.ExpectedResult, result);
        }

        // Helper handler for testing chain
        private class TestHandler : Handler
        {
            public bool WasCalled { get; private set; }
            public ExchangeRate ExpectedResult { get; } = new ExchangeRate(new Currency("USD"), new Currency("CZK"), 99.99m);

            public override ExchangeRate GetExchangeRate(Currency currency)
            {
                WasCalled = true;
                return ExpectedResult;
            }
        }
    }
}