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

            Currency currency = new("USD");
            Rate expectedRate = new("United States", "Dollar", 1, "USD", 25.50m);
            db.Add("USD", expectedRate);

            Redis redis = new();

            // Act
            ExchangeRate result = redis.GetExchangeRate(currency);

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

            Redis redis = new ();
            Currency currency = new ("USD");

            // Act
            ExchangeRate result = redis.GetExchangeRate(currency);

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

            Redis redis = new ();
            Currency currency = new ("USD");

            // Set up a mock next handler
            TestHandler nextHandler = new ();
            redis.SetNext(nextHandler);

            // Act
            ExchangeRate result = redis.GetExchangeRate(currency);

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