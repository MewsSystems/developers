namespace ExchangeRateUpdater.UnitTests
{
    [TestClass]
    public class ExchangeRateTests
    {
        [TestMethod]
        public void Equals_ShouldReturnTrue_WhenAllFieldsTheSame()
        {
            // Arrange

            var ex1 = new ExchangeRate(new Currency("USD"), new Currency("PLN"), DateTime.Today, 1, 4.3M);
            var ex2 = new ExchangeRate(new Currency("USD"), new Currency("PLN"), DateTime.Today, 1, 4.3M);

            // Act
            
            bool result = ex1.Equals(ex2);

            // Assert

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_ShouldReturnTrue_WhenTimeComponentDiffers()
        {
            // Arrange

            var ex1 = new ExchangeRate(new Currency("USD"), new Currency("PLN"), new DateTime(2024, 02, 13, 10, 10, 10), 1, 4.3M);
            var ex2 = new ExchangeRate(new Currency("USD"), new Currency("PLN"), new DateTime(2024, 02, 13, 12, 10, 10), 1, 4.3M);

            // Act

            bool result = ex1.Equals(ex2);

            // Assert

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_ShouldReturnTrue_WhenMultiplierDiffers()
        {
            // Arrange

            var ex1 = new ExchangeRate(new Currency("USD"), new Currency("PLN"), DateTime.Today, 1, 4.3M);
            var ex2 = new ExchangeRate(new Currency("USD"), new Currency("PLN"), DateTime.Today, 2, 4.3M);

            // Act

            bool result = ex1.Equals(ex2);

            // Assert

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_ShouldReturnTrue_WhenRateDiffers()
        {
            // Arrange

            var ex1 = new ExchangeRate(new Currency("USD"), new Currency("PLN"), DateTime.Today, 1, 4.3M);
            var ex2 = new ExchangeRate(new Currency("USD"), new Currency("PLN"), DateTime.Today, 1, 5.3M);

            // Act

            bool result = ex1.Equals(ex2);

            // Assert

            Assert.IsTrue(result);
        }
    }
}