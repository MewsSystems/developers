using ExchangeRateProvider.Application.Queries;
using ExchangeRateProvider.Domain.Entities;
using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Domain.Providers;
using Moq;

namespace ExchangeRateProvider.Tests.Queries
{
    public class GetExchangeRatesQueryHandlerTests
    {
        private GetExchangeRatesQueryHandler CreateHandler(IExchangeRateProvider provider = null)
        {
            var providerMock = provider != null ? Mock.Get(provider) : new Mock<IExchangeRateProvider>();
            return new GetExchangeRatesQueryHandler(provider ?? providerMock.Object);
        }

        [Fact]
        public async Task Returns_Exchange_Rates_For_Requested_Currencies()
        {
            var provider = new Mock<IExchangeRateProvider>();
            var handler = CreateHandler(provider.Object);

            var usd = new Currency("USD");
            var czk = new Currency("CZK");

            // Setup the required interface members
            provider.Setup(p => p.Name).Returns("TestProvider");
            provider.Setup(p => p.Priority).Returns(100);
            provider.Setup(p => p.CanHandle(It.IsAny<IEnumerable<Currency>>())).Returns(true);

            provider.Setup(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExchangeRate> { new ExchangeRate(usd, czk, 22m) });

            var query = new GetExchangeRatesQuery([usd], czk);
            var result = (await handler.Handle(query, CancellationToken.None)).ToList();

            Assert.Single(result);
            Assert.Equal("USD", result[0].SourceCurrency.Code);
            Assert.Equal("CZK", result[0].TargetCurrency.Code);
            Assert.Equal(22m, result[0].Value);
        }

        [Fact]
        public async Task Returns_Empty_When_No_Currencies_Requested()
        {
            var provider = new Mock<IExchangeRateProvider>();
            var handler = CreateHandler(provider.Object);

            var query = new GetExchangeRatesQuery([], new Currency("CZK"));
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }

        [Fact]
        public async Task Filters_Rates_To_Only_Requested_Currencies()
        {
            var provider = new Mock<IExchangeRateProvider>();
            var handler = CreateHandler(provider.Object);

            var usd = new Currency("USD");
            var czk = new Currency("CZK");

            // Setup the required interface members
            provider.Setup(p => p.Name).Returns("TestProvider");
            provider.Setup(p => p.Priority).Returns(100);
            provider.Setup(p => p.CanHandle(It.IsAny<IEnumerable<Currency>>())).Returns(true);

            // Provider returns rates for currencies not requested
            provider.Setup(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExchangeRate>
                {
                    new ExchangeRate(usd, czk, 22m),
                    new ExchangeRate(new Currency("EUR"), czk, 24m) // Not requested
                });

            var query = new GetExchangeRatesQuery([usd], czk);
            var result = (await handler.Handle(query, CancellationToken.None)).ToList();

            // Should only return rates for requested currencies
            Assert.Single(result);
            Assert.Equal("USD", result[0].SourceCurrency.Code);
        }

    }
}


