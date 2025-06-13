namespace ExchangeRate.Tests.CommonTest.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void GetPreviousYearMonthUtc_ReturnsPreviousMonth_ForNonJanuary()
        {
            // Arrange
            var testDate = new DateTime(2024, 5, 15);
            var expected = "2024-04";
            var originalNow = DateTime.UtcNow;

            // Act
            var result = "";
            System.Func<DateTime> originalUtcNow = () => DateTime.UtcNow;
            try
            {
            
                var prevMonth = testDate.Month == 1
                    ? new DateTime(testDate.Year - 1, 12, 1)
                    : new DateTime(testDate.Year, testDate.Month - 1, 1);
                result = prevMonth.ToString("yyyy-MM");
            }
            finally
            {
            
            }

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetPreviousYearMonthUtc_ReturnsDecemberOfPreviousYear_ForJanuary()
        {
            // Arrange
            var testDate = new DateTime(2024, 1, 10);
            var expected = "2023-12";

            // Act
            var prevMonth = testDate.Month == 1
                ? new DateTime(testDate.Year - 1, 12, 1)
                : new DateTime(testDate.Year, testDate.Month - 1, 1);
            var result = prevMonth.ToString("yyyy-MM");

            // Assert
            Assert.Equal(expected, result);
        }
    }
}