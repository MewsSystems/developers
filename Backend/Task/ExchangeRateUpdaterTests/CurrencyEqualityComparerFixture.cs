using ExchangeRateUpdater;
using Xunit;

namespace ExchangeRateUpdaterTests
{
    public class CurrencyEqualityComparerFixture
    {

        [Fact]
        public void Same_Objects_Are_Equal()
        {
            // arrange 
            var x = new Currency("USD");
            var y = new Currency("USD");
            var comparer = new CurrencyEqualityComparer();

            // act
            var actual = comparer.Equals(x, y);

            // assert 
            Assert.True(actual);
        }

        [Fact]
        public void Different_Objects_Not_Equal()
        {
            // arrange 
            var x = new Currency("USD");
            var y = new Currency("EUR");
            var comparer = new CurrencyEqualityComparer();

            // act
            var actual = comparer.Equals(x, y);

            // assert 
            Assert.False(actual);
        }
    }
}
