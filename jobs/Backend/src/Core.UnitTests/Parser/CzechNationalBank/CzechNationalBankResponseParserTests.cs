using AutoFixture.Xunit2;
using Common.Csv;
using Core.Models.CzechNationalBank;
using Core.Parser.CzechNationalBank;
using FluentAssertions;
using Moq;
using Xunit;

namespace Core.UnitTests.Parser.CzechNationalBank
{
    public class CzechNationalBankResponseParserTests
    {
        private readonly Mock<ICsvWrapper> _mockCsvWrapper;
        private readonly CzechNationalBankResponseParser _objectToTest;

        private IEnumerable<CzechNationalBankExchangeRateItem> expected
            = new List<CzechNationalBankExchangeRateItem>
            {
                new CzechNationalBankExchangeRateItem
                {
                    Country = "Australia",
                    Currency = "dollar",
                    Amount = 1,
                    Code = "AUD",
                    Rate = (decimal) 16.027
                },
                new CzechNationalBankExchangeRateItem
                {
                    Country = "Brazil",
                    Currency = "real",
                    Amount = 1,
                    Code = "BRL",
                    Rate = (decimal) 4.854
                },
            };

        public CzechNationalBankResponseParserTests()
        {
            _mockCsvWrapper = new Mock<ICsvWrapper>();
            _objectToTest = new CzechNationalBankResponseParser(_mockCsvWrapper.Object);            
        }

        public class ParseResponseTests : CzechNationalBankResponseParserTests
        {
            private void SetupParseResponseTests()
            {
                _mockCsvWrapper
                    .Setup(x => x.ParseCsv<CzechNationalBankExchangeRateItem>(It.IsAny<string>(), "|", true, true))
                    .Returns(expected);
            }

            [Theory, AutoData]
            public void Parses_RateData_From_CSV(string testCsvData)
            {
                // Arrange
                SetupParseResponseTests();

                // Act
                var actual = _objectToTest.ParseResponse(testCsvData);

                // Assert
                actual.Should().HaveSameCount(expected);
            }
        }
    }
}
