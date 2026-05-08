using ExchangeRateUpdater.Application.Validators;
using Xunit;

namespace ExchangeRateUpdater.Tests.UnitTests
{
    public class DateValidatorTests
    {
        [Theory]
        [InlineData("2020-01-01", true)]
        [InlineData("1999-12-31", true)]
        [InlineData("", false)]
        [InlineData("invalidDate", false)]
        [InlineData("01-01-2020", false)]
        public void ValidateDate_ReturnsExpectedResult_ForValidOrInvalidFormats(string input, bool expected)
        {
            // Act
            bool result = DateValidator.ValidateDate(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ValidateDate_ReturnsFalse_ForFutureDate()
        {
            // Arrange
            string futureDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

            // Act
            bool result = DateValidator.ValidateDate(futureDate);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateDate_ReturnsTrue_ForToday()
        {
            // Arrange
            string today = DateTime.Today.ToString("yyyy-MM-dd");

            // Act
            bool result = DateValidator.ValidateDate(today);

            // Assert
            Assert.True(result);
        }
    }
}
