using ExchangeRateUpdater.DataTransferObjects;
using ExchangeRateUpdater.ExchangeRateProviders;
using ExchangeRateUpdater.RateProviders;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdater.UnitTests.ExchangeRateProviders
{
    public class ExchangeRateProviderTests
    {
        private const int BaseAmmout = 1;

        private readonly IEnumerable<CurrencyRateDto> _currencyRates;
        private readonly IEnumerable<Currency> _currencies;
        private readonly Mock<IRateProvider> _mockRateProvider;

        public ExchangeRateProviderTests()
        {
            _mockRateProvider = new Mock<IRateProvider>();

            _currencyRates = new CurrencyRateDto[2]
            {
                new CurrencyRateDto()
                {
                    Ammount = 1,
                    Code = "USD",
                    CountryName = "USA",
                    Name = "dolar",
                    Rate = 21.744m
                },
                new CurrencyRateDto()
                {
                    Ammount = 1,
                    Code = "EUR",
                    CountryName = "EMU",
                    Name = "euro",
                    Rate = 24.520m
                }
            };

            _currencies = new Currency[2]
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            _mockRateProvider
                .Setup(rp => rp.GetRatesAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(_currencyRates);
            _mockRateProvider
                .Setup(rp => rp.GetRatesAsync())
                .ReturnsAsync(_currencyRates);
            _mockRateProvider
                .Setup(rp => rp.BaseAmmount)
                .Returns(BaseAmmout);
            _mockRateProvider
                .Setup(rp => rp.BaseCurrency)
                .Returns(new Currency("CZK"));
        }

        [Fact]
        public async void GetExchangeRates_UnknownCurrencies_AreIgnored()
        {
            var exchangeRateProvider = new ExchangeRateProvider(_mockRateProvider.Object);
            var uknownCurrency = new Currency("ABC");
            var unknownCurrencies = new Currency[1] { uknownCurrency };

            var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(_currencies.Concat(unknownCurrencies));

            Assert.DoesNotContain(uknownCurrency.Code, exchangeRates.Select(er => er.SourceCurrency.Code));
        }

        [Fact]
        public async void GetExchangeRates_KnownCurrencies_AreParsed()
        {

            var exchangeRateProvider = new ExchangeRateProvider(_mockRateProvider.Object);

            var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(_currencies);

            Assert.All(exchangeRates, er => Assert.Contains(_currencies, c => c.Code == er.SourceCurrency.Code));
        }

        [Fact]
        public async void GetExchangeRates_GenerateExchangeRates_RateIsCorrect()
        {
            var exchangeRateProvider = new ExchangeRateProvider(_mockRateProvider.Object);

            var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(_currencies);

            Assert.All(exchangeRates, er =>
            {
                var rate = _currencyRates.Single(cr => cr.Code == er.SourceCurrency.Code);

                Assert.Equal(rate.Rate * decimal.Divide(BaseAmmout, rate.Ammount), er.Value);
            });
        }
    }
}
