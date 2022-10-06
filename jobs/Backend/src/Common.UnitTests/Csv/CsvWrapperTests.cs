using Common.Csv;
using CsvHelper.Configuration.Attributes;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Common.UnitTests.Csv
{
    public class CsvWrapperTests
    {
        private readonly ICsvWrapper _objectToTest;
        private string testCsvData = "04 Oct 2022 #192\n\rCountry|Currency|Amount|Code|Rate\n\rAustralia|dollar|1|AUD|16.027\n\rBrazil|real|1|BRL|4.854";

        private class TestCsvModel
        {
            [Name("Country")]
            public string Country { get; set; }
            [Name("Currency")]
            public string Currency { get; set; }
            [Name("Amount")]
            public int Amount { get; set; }
            [Name("Code")]
            public string Code { get; set; }
            [Name("Rate")]
            public decimal Rate { get; set; }
        }

        public CsvWrapperTests()
        {
            _objectToTest = new CsvWrapper();
        }

        public class Csv : CsvWrapperTests
        {
            [Theory]
            [InlineData("|")]
            public void Returns_Data_If_Correct_Delimitter(string delimitter)
            {
                // Arrange

                // Act
                var actual = _objectToTest.ParseCsv<TestCsvModel>(testCsvData, delimitter, true, true);

                // Assert
                actual.Should().HaveCountGreaterThan(0);
            }

            [Theory]
            [InlineData(",")]
            public void Throws_Exception_If_Wrong_Delimitter(string delimitter)
            {
                // Arrange

                // Act
                Func<object> action = () => _objectToTest.ParseCsv<TestCsvModel>(testCsvData, delimitter, true, true);

                // Assert
                using (new AssertionScope())
                {
                    action.Should().Throw<Exception>();
                }
            }
        }
    }
}
