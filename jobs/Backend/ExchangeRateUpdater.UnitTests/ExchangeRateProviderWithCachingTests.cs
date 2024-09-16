using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.UnitTests
{
    [TestClass]
    public class ExchangeRateProviderWithCachingTests
    {
        [TestMethod]
        public async Task GetExchangeRates_ShouldReturnRequestedRatesForTheFirstTime()
        {
            // Arrange

            var requestedCurrencies = new Currency[]
            {
                new Currency("USD"),
                new Currency("PLN") 
            };

            IEnumerable<ExchangeRate> returnedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateTime.Today, 1, 4.3M),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), DateTime.Today, 1, 3.3M),
                new ExchangeRate(new Currency("PLN"), new Currency("CZK"), DateTime.Today, 10, 2.3M),
                new ExchangeRate(new Currency("CHF"), new Currency("CZK"), DateTime.Today, 100, 6.3M),
                new ExchangeRate(new Currency("AUD"), new Currency("CZK"), DateTime.Today, 1, 7.3M),
            };

            var exchangeRateProviderMock = new Mock<IExchangeRateProvider>();
            exchangeRateProviderMock.Setup(x => x.GetExchangeRates(requestedCurrencies, CancellationToken.None))
                .Returns(Task.FromResult(returnedRates));

            var rateProvider = new ExchangeRateProviderWithCaching(exchangeRateProviderMock.Object);

            IEnumerable<ExchangeRate> expectedResult = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateTime.Today, 1, 4.3M),
                new ExchangeRate(new Currency("PLN"), new Currency("CZK"), DateTime.Today, 10, 2.3M),
            };

            // Act

            IEnumerable<ExchangeRate> result = await rateProvider.GetExchangeRates(requestedCurrencies, CancellationToken.None);

            // Assert

            result.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public async Task GetExchangeRates_ShouldReturnCachedRatesForTheSecondTime()
        {
            // Arrange 1

            var requestedCurrencies = new Currency[] 
            { 
                new Currency("USD"), 
                new Currency("PLN") 
            };

            IEnumerable<ExchangeRate> firstReturnedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateTime.Today, 1, 4.3M),
                new ExchangeRate(new Currency("PLN"), new Currency("CZK"), DateTime.Today, 10, 2.3M),
            };

            var firsRequestMock = new Mock<IExchangeRateProvider>();
            firsRequestMock.Setup(x => x.GetExchangeRates(requestedCurrencies, CancellationToken.None))
                .Returns(Task.FromResult(firstReturnedRates));

            var rateProvider = new ExchangeRateProviderWithCaching(firsRequestMock.Object);

            // Act 1

            await rateProvider.GetExchangeRates(requestedCurrencies, CancellationToken.None);

            // Arrange 2

            IEnumerable<ExchangeRate> secondReturnedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateTime.Today, 1, 5.3M),
                new ExchangeRate(new Currency("PLN"), new Currency("CZK"), DateTime.Today, 10, 2.3M),
            };

            firsRequestMock.Setup(x => x.GetExchangeRates(requestedCurrencies, CancellationToken.None))
               .Returns(Task.FromResult(secondReturnedRates));

            IEnumerable<ExchangeRate> expectedResult = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateTime.Today, 1, 4.3M),
                new ExchangeRate(new Currency("PLN"), new Currency("CZK"), DateTime.Today, 10, 2.3M),
            };

            // Act 2

            IEnumerable<ExchangeRate> result = await rateProvider.GetExchangeRates(requestedCurrencies, CancellationToken.None);

            //Assert

            result.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public async Task GetExchangeRates_ShouldSkipCacheWhenNewCurrencyRequested()
        {
            // Arrange 1

            var firstRequestedCurrencies = new Currency[]
            {
                new Currency("USD"), 
            };

            IEnumerable<ExchangeRate> returnedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateTime.Today, 1, 4.3M),
                new ExchangeRate(new Currency("PLN"), new Currency("CZK"), DateTime.Today, 10, 2.3M),
            };

            var firsRequestMock = new Mock<IExchangeRateProvider>();
            firsRequestMock.Setup(x => x.GetExchangeRates(firstRequestedCurrencies, CancellationToken.None))
                .Returns(Task.FromResult(returnedRates));

            // Act 1

            var rateProvider = new ExchangeRateProviderWithCaching(firsRequestMock.Object);
            await rateProvider.GetExchangeRates(firstRequestedCurrencies, CancellationToken.None);

            // Arrange 2
            var secondRequestedCurrencies = new Currency[]
            {
                new Currency("USD"),
                new Currency("PLN"),
            };

            IEnumerable<ExchangeRate> expectedResult = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateTime.Today, 1, 4.3M),
                new ExchangeRate(new Currency("PLN"), new Currency("CZK"), DateTime.Today, 10, 2.3M),
            };

            // Act 2

            IEnumerable<ExchangeRate> result = await rateProvider.GetExchangeRates(secondRequestedCurrencies, CancellationToken.None);

            // Assert

            result.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public async Task GetExchangeRates_ShouldSkipCacheWhenRequestedCurrencyWasNotReturned()
        {
            // Arrange 1

            var requestedCurrencies = new Currency[]
            {
                new Currency("USD"),
                new Currency("PLN"),
            };

            IEnumerable<ExchangeRate> firstReturnedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateTime.Today, 1, 4.3M),
            };

            var firsRequestMock = new Mock<IExchangeRateProvider>();
            firsRequestMock.Setup(x => x.GetExchangeRates(requestedCurrencies, CancellationToken.None))
                .Returns(Task.FromResult(firstReturnedRates));

            // Act 1

            var rateProvider = new ExchangeRateProviderWithCaching(firsRequestMock.Object);
            await rateProvider.GetExchangeRates(requestedCurrencies, CancellationToken.None);

            // Arrange 2

            IEnumerable<ExchangeRate> secondReturnedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateTime.Today, 1, 4.3M),
                new ExchangeRate(new Currency("PLN"), new Currency("CZK"), DateTime.Today, 10, 2.3M),
            };

            firsRequestMock.Setup(x => x.GetExchangeRates(requestedCurrencies, CancellationToken.None))
            .Returns(Task.FromResult(secondReturnedRates));

            IEnumerable<ExchangeRate> expectedResult = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateTime.Today, 1, 4.3M),
                new ExchangeRate(new Currency("PLN"), new Currency("CZK"), DateTime.Today, 10, 2.3M),
            };

            // Act 2

            IEnumerable<ExchangeRate> result = await rateProvider.GetExchangeRates(requestedCurrencies, CancellationToken.None);

            // Assert

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
