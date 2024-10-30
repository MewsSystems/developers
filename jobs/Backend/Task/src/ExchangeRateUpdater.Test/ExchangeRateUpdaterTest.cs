using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Ack;
using ExchangeRateUpdater.Domain.Config;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Model.Cnb.Rs;
using ExchangeRateUpdater.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.Test
{
    public class ExchangeRateUpdaterTest
    {
        private readonly Mock<IHttpClientService> httpClientServiceMock;
        private readonly Mock<ILogger<ExchangeRateProvider>> loggerMock;

        private readonly ExchangeRateProvider exchangeRateProvider;
        private readonly CnbApiConfig cnbApiConfig;

        public ExchangeRateUpdaterTest()
        {
            httpClientServiceMock = new Mock<IHttpClientService>();
            cnbApiConfig = new CnbApiConfig
            {
                BaseUrl = "https://link.com",
                ClientName = "test",
                ExchangeRateApiUrl = "/success",
                LocalCurrencyIsoCode = "CZK",
                PreferredLanguage = "EN",
                TimeOut = 30
            };

            var optionsCnbApiConfig = Options.Create(cnbApiConfig);

            loggerMock = new Mock<ILogger<ExchangeRateProvider>>();

            exchangeRateProvider = new ExchangeRateProvider(httpClientServiceMock.Object,
                                                            optionsCnbApiConfig,
                                                            loggerMock.Object);
        }

        [Fact]
        public async Task GetExchangeRates_WithFewCurrencies_ReturnsCorrectExchangeRates()
        {
            IReadOnlyCollection<Currency> currencies =
            [
                new Currency("CZK"), new Currency("AUD"),
            ];

            MoqHttpClientServiceGetAsyncInstance(new AckEntity<CnbExchangeRatesRsModel>(true, new CnbExchangeRatesRsModel
            {
                Rates =
                [
                    new()
                    {
                        Amount = 100,
                        CurrencyCode = "AUD",
                        Rate = 2555M
                    },
                ]
            }));

            var result = await exchangeRateProvider.GetExchangeRates(currencies);

            Assert.Single(result);
            Assert.Equal("AUD/CZK=25,55", result.Single().ToString());
        }

        [Fact]
        public async Task GetExchangeRates_WithManyValidInputs_ReturnsCorrectValue()
        {
            IReadOnlyCollection<Currency> currencies =
            [
                new Currency("CZK"),
                new Currency("ZAR"),
                new Currency("KRW"),
                new Currency("SEK"),
                new Currency("CHF"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("GBP"),
                new Currency("USD")
            ];

            var responseCurrencies = new List<CnbExchangeRatesRsModel.CnbExchangeRatesRsModelRate> {
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "ZAR",
                        Rate = 1.325M
                    },
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "KRW",
                        Rate = 1.7M
                    },
                    new()
                    {
                        Amount = 100,
                        CurrencyCode = "SEK",
                        Rate = 219.3M
                    },
                    new()
                    {
                        Amount = 100,
                        CurrencyCode = "THB",
                        Rate = 69.475M
                    },
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "CHF",
                        Rate = 27.017M
                    },
                    new()
                    {
                        Amount = 100,
                        CurrencyCode = "TRY",
                        Rate = 68.435M
                    },
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "GBP",
                        Rate = 30.438M
                    },
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "USD",
                        Rate = 23.466M
                    }
            };

            MoqHttpClientServiceGetAsyncInstance(new AckEntity<CnbExchangeRatesRsModel>(true, new CnbExchangeRatesRsModel
            {
                Rates = [.. responseCurrencies]
            }));

            var result = await exchangeRateProvider.GetExchangeRates(currencies);

            Assert.All(responseCurrencies, expectedCurrency =>
            {
                var expValue = expectedCurrency.Rate / expectedCurrency.Amount;
                var actualExchange = result.FirstOrDefault(x => x.SourceCurrency.Code == expectedCurrency.CurrencyCode);

                Assert.NotNull(actualExchange);
                Assert.True(actualExchange.TargetCurrency.Code == "CZK");
            });
        }

        [Fact]
        public async Task GetExchangeRates_WithInvalidCurrencies_ReturnsEmptyList()
        {
            IReadOnlyCollection<Currency> currencies =
            [
                new Currency("CZK"), new Currency("XRY"),
            ];

            MoqHttpClientServiceGetAsyncInstance(new AckEntity<CnbExchangeRatesRsModel>(true, new CnbExchangeRatesRsModel
            {
                Rates =
                [
                    new()
                    {
                        Amount = 100,
                        CurrencyCode = "AUD",
                        Rate = 2555M
                    },
                ]
            }));

            var result = await exchangeRateProvider.GetExchangeRates(currencies);

            Assert.Empty(result);
        }
        
        [Fact]
        public async Task GetExchangeRates_WithoutLocalCurrency_ReturnsEmptyList()
        {
            IReadOnlyCollection<Currency> currencies =
            [
                new Currency("ZAR"),
                new Currency("KRW"),
                new Currency("SEK"),
                new Currency("CHF"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("GBP"),
                new Currency("USD")
            ];

            MoqHttpClientServiceGetAsyncInstance(new AckEntity<CnbExchangeRatesRsModel>(true, new CnbExchangeRatesRsModel
            {
                Rates =
                [
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "ZAR",
                        Rate = 1.325M
                    },
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "KRW",
                        Rate = 1.7M
                    },
                    new()
                    {
                        Amount = 100,
                        CurrencyCode = "SEK",
                        Rate = 219.3M
                    },
                    new()
                    {
                        Amount = 100,
                        CurrencyCode = "THB",
                        Rate = 69.475M
                    },
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "CHF",
                        Rate = 27.017M
                    },
                    new()
                    {
                        Amount = 100,
                        CurrencyCode = "TRY",
                        Rate = 68.435M
                    },
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "GBP",
                        Rate = 30.438M
                    },
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "USD",
                        Rate = 23.466M
                    }
                ]
            }));

            var result = await exchangeRateProvider.GetExchangeRates(currencies);

            Assert.Empty(result);

            httpClientServiceMock.Verify(dep => dep.GetAsync<CnbExchangeRatesRsModel>(
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            VerifyLoggerMessage("Not enough currencies has been found on the request.");
        }

        [Fact]
        public async Task GetExchangeRates_WhenSourceApiReturnsError_ReturnsEmptyList()
        {
            IReadOnlyCollection<Currency> currencies =
            [
                new Currency("CZK"), new Currency("AUD"),
            ];

            MoqHttpClientServiceGetAsyncInstance(new AckEntity<CnbExchangeRatesRsModel>(false, null, "Error"));

            var result = await exchangeRateProvider.GetExchangeRates(currencies);

            Assert.Empty(result);
            VerifyLoggerMessage("No exchange rates retrieved from the source.");
        }

        [Fact]
        public async Task GetExchangeRates_WhenSourceApiNullResponse_ReturnsEmptyList()
        {
            IReadOnlyCollection<Currency> currencies =
            [
                new Currency("CZK"), new Currency("AUD"),
            ];

            MoqHttpClientServiceGetAsyncInstance(null);

            var result = await exchangeRateProvider.GetExchangeRates(currencies);

            Assert.Empty(result);
            VerifyLoggerMessage("No exchange rates retrieved from the source.");
        }

        private void MoqHttpClientServiceGetAsyncInstance(AckEntity<CnbExchangeRatesRsModel> returnObject)
        {
            httpClientServiceMock
                   .Setup(dep => dep.GetAsync<CnbExchangeRatesRsModel>(
                       It.IsAny<string>(),
                       It.IsAny<string>()))
                   .ReturnsAsync(returnObject);
        }

        private void VerifyLoggerMessage(string message, LogLevel logLevel = LogLevel.Warning)
        {
            loggerMock.Verify(
                x => x.Log(
                    logLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}