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
                new ExchangeRate(new Currency("USD"), new Currency("EUR"), 2),
                new ExchangeRate(new Currency("JPY"), new Currency("EUR"), 3),
                new ExchangeRate(new Currency("RUB"), new Currency("EUR"), 4),
                new ExchangeRate(new Currency("THB"), new Currency("EUR"), 5)
            };
        }

        [Theory]
        [InlineData("USD,RUB", 2)]
        [InlineData("USD,RUB,THB,JPY,AYZ,FFF", 4)]
        [InlineData("USD,RUB,AYZ", 2)]
        [InlineData("FFF,ASD,AYZ", 0)]
        public void When_Filtering_List_By_Source_Codes_Then_Return_FilteredList(
            string sourceCurrencies, int expectedCount)
        {
            // Arrange
            var sourceCurrencyList = sourceCurrencies.Split(',');

            // Act
            var filteredResult = _exchangeRates.FilterBySourceCurrency(sourceCurrencyList);

            // Assert
            filteredResult.Count().Should().Be(expectedCount);
            filteredResult.All(result => sourceCurrencyList.Contains(result.SourceCurrency.Code));
        }
    }
}
