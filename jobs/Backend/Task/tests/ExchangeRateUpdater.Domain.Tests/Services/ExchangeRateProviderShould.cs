using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace ExchangeRateUpdater.Domain.Tests.Services
{
    public class ExchangeRateProviderShould
    {
        private static readonly ExchangeRate[] _allExchangeRates =
            [
                (new ExchangeRate(new Currency("AUD"), new Currency("CZK"), 15.427m)),
                (new ExchangeRate(new Currency("BRL"), new Currency("CZK"), 4.653m)),
                (new ExchangeRate(new Currency("CAD"), new Currency("CZK"), 17.296m)),
                (new ExchangeRate(new Currency("DKK"), new Currency("CZK"), 3.395m)),
                (new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25.330m)),
                (new ExchangeRate(new Currency("GBP"), new Currency("CZK"), 29.651m))
            ];

        [Fact]
        public async Task ReturnAllExchangeRatesWhenCurrenciesListIsEmpty()
        {
            //Arrange
            var exchangeRepository = Substitute.For<IExchangeRatesRepository>();
            exchangeRepository
                .GetExchangeRatesAsync(Arg.Any<DateOnly?>())
                .Returns(Task.FromResult(_allExchangeRates.AsEnumerable()));

            var exchangeRateprovider = new ExchangeRateProvider(exchangeRepository);

            //Act
            var result = await exchangeRateprovider.GetExchangeRatesAsync(date: null, currencies: []);

            //Assert
            result.Should().BeEquivalentTo(_allExchangeRates, options => options.WithStrictOrdering());
        }

        [Fact]
        public async Task FilterExchangeRatesBaseOnCurrencies()
        {
            //Arrange
            var exchangeRepository = Substitute.For<IExchangeRatesRepository>();
            exchangeRepository
                .GetExchangeRatesAsync(Arg.Any<DateOnly?>())
                .Returns(Task.FromResult(_allExchangeRates.AsEnumerable()));

            var exchangeRateprovider = new ExchangeRateProvider(exchangeRepository);

            var currencies = new[]
            {
                new Currency("EUR"),
                new Currency("GBP")
            };
            var expected = new[]
            {
                (new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25.330m)),
                (new ExchangeRate(new Currency("GBP"), new Currency("CZK"), 29.651m))
            };

            //Act
            var result = await exchangeRateprovider.GetExchangeRatesAsync(date: null, currencies);

            //Assert
            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }
    }
}
