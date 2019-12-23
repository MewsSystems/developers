using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class CnbExchangeRateProviderTest
    {
        private readonly CnbExchangeRateProvider _provider;

        public CnbExchangeRateProviderTest()
        {
            //don't care about the validity of inserted CNB url, because we are not testing the API output here
            var cnbClient = Mock.Of<ICnbClient>(x => x.ReadExchangeRatesFromUrlAsync(It.IsAny<string>()) == 
            Task.FromResult<IEnumerable<CnbXmlRow>>(new List<CnbXmlRow>
            {
                new CnbXmlRow { CurrencyCode = "AUD", CurrencyName = "dolar", Amount = 1, ExchangeRate = 15.820M, Country = "Austrálie" },
                new CnbXmlRow { CurrencyCode = "BRL", CurrencyName = "real", Amount = 1, ExchangeRate = 5.634M, Country = "Brazílie" },
                new CnbXmlRow { CurrencyCode = "PHP", CurrencyName = "peso", Amount = 100, ExchangeRate = 45.223M, Country = "Filipíny" },
                new CnbXmlRow { CurrencyCode = "USD", CurrencyName = "dolar", Amount = 0, ExchangeRate = 10M, Country = "USA" }
            }));

            _provider = new CnbExchangeRateProvider(cnbClient);
        }

        public static IEnumerable<object[]> InvalidCurrencies =>
            new List<object[]>
            {
                new object[] { new List<Currency>() },
                new object[] { null },
                new object[] { new List<Currency>() { new Currency("XYZ") } }
            };

        [Theory]
        [MemberData(nameof(InvalidCurrencies))]
        public void GetExchangeRates_InvalidParameters_Empty(IEnumerable<Currency> currencies)
        {
            Assert.Empty(_provider.GetExchangeRates(currencies));
        }

        [Fact]
        public void GetExchangeRates_ValidParameters_NotEmpty()
        {
            Assert.NotEmpty(_provider.GetExchangeRates(new Currency[] { new Currency("AUD") }));
        }

        [Fact]
        public void GetExchangeRates_ValidParameters_DivideByZero()
        {
            Assert.All(_provider.GetExchangeRates(new Currency[] 
            {
                new Currency("USD")
            }), x => Assert.True(x.Value > 0));
        }

        [Fact]
        public void GetExchangeRates_ValidParameters_DividedByAmount()
        {
            Assert.Contains(_provider.GetExchangeRates(new Currency[] 
            {
                new Currency("PHP"),
                new Currency("BRL")
            }), x => x.Value == 0.45223M || x.Value == 5.634M);
        }
    }
}
