using FluentAssertions;
using MewsFinance.Application.Extensions;
using MewsFinance.Domain.Models;

namespace MewsFinance.Application.UnitTests.Extensions
{
    public class ExchangeRateEnumerableExtensionTests
    {
        IEnumerable<ExchangeRate> _exchangeRates;

        public ExchangeRateEnumerableExtensionTests()
        {
            _exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("EUR"), 2, 1),
                new ExchangeRate(new Currency("JPY"), new Currency("EUR"), 3, 1),
                new ExchangeRate(new Currency("RUB"), new Currency("EUR"), 4, 100),
                new ExchangeRate(new Currency("THB"), new Currency("EUR"), 5, 1)
            };
        }

        [Theory]
        [InlineData("USD,RUB", 1)]
        [InlineData("USD,RUB,THB,JPY,AYZ,FFF", 3)]
        [InlineData("USD,THB,AYZ", 2)]
        [InlineData("FFF,ASD,AYZ", 0)]
        [InlineData("RUB", 0)]
        public void When_Filtering_List_By_Source_Codes_And_Currency_Unit_Amount_Then_Return_FilteredList(
            string sourceCurrencies, int expectedCount)
        {
            // Arrange
            var sourceCurrencyList = sourceCurrencies.Split(',');

            // Act
            var filteredResult = _exchangeRates.FilterBySourceCurrencyAndUnitAmount(sourceCurrencyList);

            // Assert
            filteredResult.Count().Should().Be(expectedCount);
            filteredResult.All(result => sourceCurrencyList.Contains(result.SourceCurrency.Code));
        }
    }
}
