using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Ack;
using ExchangeRateUpdater.Domain.Config;
using ExchangeRateUpdater.Domain.Helpers;
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
        public async Task GetExchangeRates_WhenGivenFewCurrencies_ReturnsExpectedExchangeRate()
        {
            var currencies = CurrencyHelper.GenerateCurrencies("CZK", "AUD");
            const string ExpectedRateOutput = "AUD/CZK=25,55";

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
            Assert.Equal(ExpectedRateOutput, result.Single().ToString());
        }

        [Fact]
        public async Task GetExchangeRates_WhenGivenMultipleValidCurrencies_ReturnsExpectedExchangeRates()
        {
            var currencies = CurrencyHelper.GenerateCurrencies("CZK", "ZAR", "KRW", "SEK", "CHF", "THB", "TRY", "GBP", "USD");

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
        public async Task GetExchangeRates_WhenGivenInvalidCurrencies_ReturnsEmptyList()
        {

            var currencies = CurrencyHelper.GenerateCurrencies("CZK", "XRY");

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
        public async Task GetExchangeRates_WhenLocalCurrencyIsMissing_ReturnsEmptyList()
        {
            var currencies = CurrencyHelper.GenerateCurrencies("ZAR", "KRW", "SEK", "CHF", "THB", "TRY", "GBP", "USD");
            
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
        public async Task GetExchangeRates_WhenApiResponseIsError_ReturnsEmptyListAndLogsWarning()
        {
            var currencies = CurrencyHelper.GenerateCurrencies("CZK", "AUD");

            MoqHttpClientServiceGetAsyncInstance(new AckEntity<CnbExchangeRatesRsModel>(false, null, "Error"));

            var result = await exchangeRateProvider.GetExchangeRates(currencies);

            Assert.Empty(result);
            VerifyLoggerMessage("No exchange rates retrieved from the source.");
        }

        [Fact]
        public async Task GetExchangeRates_WhenApiResponseIsNull_ReturnsEmptyListAndLogsWarning()
        {
            var currencies = CurrencyHelper.GenerateCurrencies("CZK", "AUD");

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