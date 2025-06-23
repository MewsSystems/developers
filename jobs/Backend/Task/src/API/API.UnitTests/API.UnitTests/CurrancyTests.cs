using API.Models;

namespace API.UnitTests
{
    public class CurrencyTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCodeProperty()
        {
            // Arrange
            string code = "CZK";

            // Act
            var currency = new Currency(code);

            // Assert
            Assert.Equal(code, currency.Code);
        }

        [Fact]
        public void ToString_ShouldReturnCode()
        {
            // Arrange
            string code = "CZK";
            var currency = new Currency(code);
            string expectedString = "CZK";

            // Act
            var result = currency.ToString();

            // Assert
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsNull()
        {
            // Arrange
            string? code = null;

            // Act & Assert
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.Throws<ArgumentException>(() => new Currency(code));
#pragma warning restore CS8604 // Possible null reference argument.
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsEmpty()
        {
            // Arrange
            string code = string.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Currency(code));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsTooShort()
        {
            // Arrange
            string code = "X";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Currency(code));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsTooLong()
        {
            // Arrange
            string code = "ABCD";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Currency(code));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsLowerCase()
        {
            // Arrange
            string code = "czk";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Currency(code));
        }
    }
}
