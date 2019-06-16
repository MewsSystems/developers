using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using Moq;
using Xunit;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateProviderByCnbTest
    {

        [Fact]
        public void GetExchangeRates_Return_One()
        {
            // arrange
            var mock = new Mock<ExchangeRateProviderByCnb>(string.Empty);
            mock.Setup(xrp => xrp.GetRawExchangeRates(It.IsAny<string>()))
                .Returns("14 Jun 2019 #114\r\nCountry|Currency|Amount|Code|Rate\r\nUSA|dollar|1|USD|22.674");

            var sut = mock.Object;
            var currencies = new[]
            {
                new Currency("USD"),
            };
            var expected = new ExchangeRate(new Currency("CZK"), new Currency("USD"), new decimal(22.674));
            

            // act 
            var actual = sut.GetExchangeRates(currencies)
                .ToList();

            // assert
            Assert.NotEmpty(actual);
            Assert.Collection(actual, er =>
            {
                Assert.Equal(er.TargetCurrency, expected.TargetCurrency, new CurrencyEqualityComparer());
                Assert.Equal(er.Value, expected.Value);
            });

        }


        [Fact]
        public void GetExchangeRates_Exception_OutOfRange()
        {
            // arrange
            var mock = new Mock<ExchangeRateProviderByCnb>(MockBehavior.Strict,string.Empty);
            mock.Setup(xrp => xrp.GetRawExchangeRates(It.IsAny<string>()))
                .Returns("14 Jun 2019 #114\r\nCountry|Currency|Amount|Code|Rate\r\nUSA|dollar");

            var sut = mock.Object;

            var currencies = new[]
            {
                new Currency("USD"),
            };

            // act 
            IEnumerable<ExchangeRate> Actual() => sut.GetExchangeRates(currencies)
                .ToList();
                

            // assert
            Assert.ThrowsAny<ArgumentOutOfRangeException>(Actual);

        }

        
    }
}
