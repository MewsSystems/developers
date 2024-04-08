using ExchangeRateProvider;
using ExchangeRateProvider.Constants;
using ExchangeRateProvider.Interfaces;
using ExchangeRateProvider.Models;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRate.Tests
{
    public class CnbExchangeRateProviderTest
    {
        [Fact]
        public async Task GetExchangeRatesAsync_WhenNoRatesReturned_ReturnsEmpty()
        {
            //arrange
            var currencies = new CurrencyModel[]
            {
                new(Currency.Czk),
                new(Currency.Euro),
                new(Currency.Dollar)
            };
            var cnbHttpClientMock = new Mock<ICnbHttpClient>();
            cnbHttpClientMock
                .Setup(m => m.GetCzkExchangeRatesAsync(It.IsAny<DateTime>(), Language.Czech, new CancellationToken()))
                .ReturnsAsync(new CnbRatesModel { Rates = null });
            var provider = new CnbExchangeRateProvider(cnbHttpClientMock.Object);

            //act
            var result = await provider.GetExchangeRatesAsync(currencies);

            //assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_OnlySourceCurrenciesAreReturned()
        {
            //arrange
            var currencies = new CurrencyModel[]
            {
                new(Currency.Euro)
            };
            var cnbHttpClientMock = new Mock<ICnbHttpClient>();
            cnbHttpClientMock
                .Setup(m => m.GetCzkExchangeRatesAsync(It.IsAny<DateTime>(), Language.Czech, new CancellationToken()))
                .ReturnsAsync(new CnbRatesModel
                {
                    Rates =
                    [
                        new CnbRatesModel.RateModel
                        {
                            Amount = 1,
                            Country = "EMU",
                            Currency = "euro",
                            CurrencyCode = Currency.Euro,
                            Order = 66,
                            Rate = 25.31M,
                            ValidFor = DateTime.UtcNow
                        },
                        new CnbRatesModel.RateModel
                        {
                            Amount = 1,
                            Country = "Denmark",
                            Currency = "krone",
                            CurrencyCode = "DKK",
                            Order = 66,
                            Rate = 3.393M,
                            ValidFor = DateTime.UtcNow
                        }
                    ]
                });
            var provider = new CnbExchangeRateProvider(cnbHttpClientMock.Object);

            //act
            var result = await provider.GetExchangeRatesAsync(currencies);

            //assert
            Assert.NotNull(result);
            Assert.Single(result);
            var returnedRate = result.First();
            Assert.NotNull(returnedRate.SourceCurrency);
            Assert.Equal(Currency.Euro, returnedRate.SourceCurrency.Code);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WhenExchangeRateDoesNotExist_ReturnsEmpty()
        {
            //arrange
            var currencies = new CurrencyModel[]
            {
                new(Currency.Xyz)
            };
            var cnbHttpClientMock = new Mock<ICnbHttpClient>();
            cnbHttpClientMock
                .Setup(m => m.GetCzkExchangeRatesAsync(It.IsAny<DateTime>(), Language.Czech, new CancellationToken()))
                .ReturnsAsync(new CnbRatesModel
                {
                    Rates =
                    [
                        new CnbRatesModel.RateModel
                        {
                            Amount = 1,
                            Country = "EMU",
                            Currency = "euro",
                            CurrencyCode = Currency.Euro,
                            Order = 66,
                            Rate = 25.31M,
                            ValidFor = DateTime.UtcNow
                        },
                        new CnbRatesModel.RateModel
                        {
                            Amount = 1,
                            Country = "Denmark",
                            Currency = "krone",
                            CurrencyCode = "DKK",
                            Order = 66,
                            Rate = 3.393M,
                            ValidFor = DateTime.UtcNow
                        }
                    ]
                });
            var provider = new CnbExchangeRateProvider(cnbHttpClientMock.Object);

            //act
            var result = await provider.GetExchangeRatesAsync(currencies);

            //assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}