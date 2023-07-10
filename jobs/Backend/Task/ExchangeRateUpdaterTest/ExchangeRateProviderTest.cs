using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.CnbProvider.Abstractions;
using ExchangeRateUpdater.Domain;
using Moq;

namespace ExchangeRateUpdaterTest
{
    public class ExchangeRateProviderTest
    {
        private readonly Mock<ICnbRateProvider> _cnbRatesProvider;
        private readonly ExchangeRateProvider sut;

        public ExchangeRateProviderTest()
        {
            _cnbRatesProvider = new Mock<ICnbRateProvider>();
            sut = new ExchangeRateProvider(_cnbRatesProvider.Object);
        }


        [Fact]
        public async void WhenReceivesAnEmptyCurrencyListShouldReturnAnEmptyExchangeRateList()
        {
            //Arrange
            IEnumerable<Currency> currencies = Array.Empty<Currency>();

            //Act
            var exchangeRates = await sut.GetExchangeRatesAsync(currencies, DateTime.UtcNow);

            //Assert
            _cnbRatesProvider.Verify(v => v.GetRatesByDateAsync(It.IsAny<DateTime>()), Times.Never);
            Assert.True(exchangeRates == Enumerable.Empty<ExchangeRate>());
        }


        [Fact]
        public async void WhenReceivesANullShouldReturnAEmptyList()
        {
            //Arrange
            IEnumerable<Currency>? currencies = null;

            //Act
            var exchangeRates = await sut.GetExchangeRatesAsync(currencies, DateTime.UtcNow);

            //Assert
            _cnbRatesProvider.Verify(v => v.GetRatesByDateAsync(It.IsAny<DateTime>()), Times.Never);
            Assert.True(exchangeRates == Enumerable.Empty<ExchangeRate>());
        }


        [Fact]
        public async void WhenReceiveAListOfCurrenciesShouldReturnAExchangeRateList()
        {
            //Arrange
            IEnumerable<Currency>? currencies = new[]
                {
                    new Currency("EUR"),
                    new Currency("USD")
                };
            _cnbRatesProvider
                .Setup(s => s.GetRatesByDateAsync(new DateTime(2023, 07, 05)))
                .ReturnsAsync(new[] {
                                        new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 3),
                                        new ExchangeRate(new Currency("USD"), new Currency("CZK"), 5)
                                    });

            //Act
            var exchangeRates = await sut.GetExchangeRatesAsync(currencies, new DateTime(2023, 07, 05));

            //Assert
            _cnbRatesProvider.Verify(v => v.GetRatesByDateAsync(It.IsAny<DateTime>()), Times.Once);
            Assert.NotNull(exchangeRates);
            Assert.NotEmpty(exchangeRates);
            Assert.True(exchangeRates.First(f => f.SourceCurrency.Code == "EUR" && f.TargetCurrency.Code == "CZK").Value == 3);
            Assert.True(exchangeRates.First(f => f.SourceCurrency.Code == "USD" && f.TargetCurrency.Code == "CZK").Value == 5);
        }

        
        [Fact]
        public async void WhenReceiveACurrencyThatNotHaveRateShouldReturnAExchangeRateListWithoutThisCurrency()
        {
            //Arrange
            IEnumerable<Currency>? currencies = new[]
                {
                    new Currency("EUR"),
                    new Currency("USD"),
                    new Currency("JPY"),
                };
            _cnbRatesProvider
                .Setup(s => s.GetRatesByDateAsync(new DateTime(2023, 07, 05)))
                .ReturnsAsync(new[] {
                                        new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 3),
                                        new ExchangeRate(new Currency("USD"), new Currency("CZK"), 5)
                                    });

            //Act
            var exchangeRates = await sut.GetExchangeRatesAsync(currencies, new DateTime(2023, 07, 05));

            //Assert
            _cnbRatesProvider.Verify(v => v.GetRatesByDateAsync(It.IsAny<DateTime>()), Times.Once);
            Assert.NotNull(exchangeRates);
            Assert.True(exchangeRates.Any());
            Assert.True(exchangeRates.First(f => f.SourceCurrency.Code == "EUR" && f.TargetCurrency.Code == "CZK").Value == 3);
            Assert.True(exchangeRates.First(f => f.SourceCurrency.Code == "USD" && f.TargetCurrency.Code == "CZK").Value == 5);
            Assert.DoesNotContain(exchangeRates, f => f.SourceCurrency.Code == "JPY" && f.TargetCurrency.Code == "CZK");
        }


        [Fact]
        public async void WhenReceiveAListOfCurrenciesShouldReturnOnlyExchangeRatesForTheseCurrencies()
        {
            //Arrange
            IEnumerable<Currency>? currencies = new[]
                {
                    new Currency("EUR"),
                    new Currency("USD"),
                    new Currency("JPY"),
                };
            _cnbRatesProvider
                .Setup(s => s.GetRatesByDateAsync(new DateTime(2023, 07, 05)))
                .ReturnsAsync(new[] {
                                        new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 3),
                                        new ExchangeRate(new Currency("USD"), new Currency("CZK"), 5),
                                        new ExchangeRate(new Currency("JPY"), new Currency("CZK"), 6),
                                        new ExchangeRate(new Currency("RUB"), new Currency("CZK"), 2.5m),
                                        new ExchangeRate(new Currency("THB"), new Currency("CZK"), 6.7m),
                                        new ExchangeRate(new Currency("TRY"), new Currency("CZK"), 3),
                                        new ExchangeRate(new Currency("XYZ"), new Currency("CZK"), 1.23m),
                                    });

            //Act
            var exchangeRates = await sut.GetExchangeRatesAsync(currencies, new DateTime(2023, 07, 05));

            //Assert
            _cnbRatesProvider.Verify(v => v.GetRatesByDateAsync(It.IsAny<DateTime>()), Times.Once);
            Assert.NotNull(exchangeRates);
            Assert.NotEmpty(exchangeRates);
            Assert.True(exchangeRates.First(f => f.SourceCurrency.Code == "EUR" && f.TargetCurrency.Code == "CZK").Value == 3);
            Assert.True(exchangeRates.First(f => f.SourceCurrency.Code == "USD" && f.TargetCurrency.Code == "CZK").Value == 5);
            Assert.True(exchangeRates.First(f => f.SourceCurrency.Code == "JPY" && f.TargetCurrency.Code == "CZK").Value == 6);
            Assert.True(exchangeRates.Count() == 3);
        }
    }
}
