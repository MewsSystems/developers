using ExchangeRateUpdater.Exchange_Providers.Comparers;
using ExchangeRateUpdater.Exchange_Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
    public class CurrencyEqualityComparer_Tests
    {
        [Fact]
        public void Equals_ReturnsTrueForEqualCurrencies()
        {
            // Arrange
            var comparer = new CurrencyEqualityComparer();
            var currencyA = new Currency("USD");
            var currencyB = new Currency("USD");

            // Act
            var result = comparer.Equals(currencyA, currencyB);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_ReturnsFalseForDifferentCurrencies()
        {
            // Arrange
            var comparer = new CurrencyEqualityComparer();
            var currencyA = new Currency("USD");
            var currencyB = new Currency("EUR");

            // Act
            var result = comparer.Equals(currencyA, currencyB);

            // Assert
            Assert.False(result);
        }
    }
}
