using Infrastructure.Clients.CnbApi;
using Infrastructure.Clients.CnbApi.Basetypes;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Infrastructure.Test.Repositories
{
    public class ExchangeRateFromApiRepositoryTest
    {

        private readonly ILogger<ExchangeRateFromApiRepository> _logger;

        private readonly IOptions<ExchangeRateRepositoryConfiguration> _configuration;

        public ExchangeRateFromApiRepositoryTest()
        {
            _logger = Substitute.For<ILogger<ExchangeRateFromApiRepository>>();
            _configuration = Substitute.For<IOptions<ExchangeRateRepositoryConfiguration>>();
            _configuration.Value.Returns(new ExchangeRateRepositoryConfiguration()
            {
                TargetCurrency = "CZK"
            });
        }

        [Fact]
        public async Task ExchangeRateFromApiRepository_GetTodayCZKExchangeRatesDictionaryAsync_ResponseIsNull()
        {

            RatesResponse ratesResponse = null;

            ICnbApiClient apiClientMock = Substitute.For<ICnbApiClient>();
            apiClientMock.GetExchangeRatesDaily(Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult<RatesResponse>(
                    ratesResponse
                ));

            var actor = new ExchangeRateFromApiRepository(apiClientMock, _logger, _configuration);

            var action = await actor.GetTodayCZKExchangeRatesDictionaryAsync();

            Assert.NotNull(action);
            Assert.Empty(action);

        }

        [Fact]
        public async Task ExchangeRateFromApiRepository_GetTodayCZKExchangeRatesDictionaryAsync_ResponseIsEmpty()
        {

            RatesResponse ratesResponse = new RatesResponse()
            {
                Rates = new List<RateItem>()
            };

            ICnbApiClient apiClientMock = Substitute.For<ICnbApiClient>();
            apiClientMock.GetExchangeRatesDaily(Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult<RatesResponse>(
                    ratesResponse
                ));

            var actor = new ExchangeRateFromApiRepository(apiClientMock, _logger, _configuration);

            var action = await actor.GetTodayCZKExchangeRatesDictionaryAsync();

            Assert.NotNull(action);
            Assert.Empty(action);

        }

        [Fact]
        public async Task ExchangeRateFromApiRepository_GetTodayCZKExchangeRatesDictionaryAsync_ValidValues()
        {

            RatesResponse ratesResponse = new RatesResponse()
            {
                Rates = new List<RateItem>()
                {
                    new RateItem()
                    {
                        Amount = 1,
                        CurrencyCode = "EUR",
                        Rate = (decimal) 1.1
                    },
                    new RateItem()
                    {
                        Amount = 10,
                        CurrencyCode = "GBP",
                        Rate = (decimal) 10.1
                    }
                }
            };

            ICnbApiClient apiClientMock = Substitute.For<ICnbApiClient>();
            apiClientMock.GetExchangeRatesDaily(Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult<RatesResponse>(
                    ratesResponse
                ));

            var actor = new ExchangeRateFromApiRepository(apiClientMock, _logger, _configuration);

            var action = await actor.GetTodayCZKExchangeRatesDictionaryAsync();

            Assert.NotNull(action);

            Assert.True(action.Count == 2);

            var eurItem = action["EUR"];

            Assert.True(eurItem.Value.Value == (decimal)1.1 && eurItem.SourceCurrency.Code == "EUR");

            var gbpItem = action["GBP"];

            Assert.True(gbpItem.Value.Value == (decimal)1.01 && gbpItem.SourceCurrency.Code == "GBP");
        }

        [Fact]
        public async Task ExchangeRateFromApiRepository_GetTodayCZKExchangeRatesDictionaryAsync_ValidValuesWithInvalidItem()
        {

            RatesResponse ratesResponse = new RatesResponse()
            {
                Rates = new List<RateItem>()
                {
                    new RateItem()
                    {
                        Amount = 1,
                        CurrencyCode = "EUR",
                        Rate = (decimal) 1.1
                    },
                    new RateItem()
                    {
                        Amount = 10,
                        CurrencyCode = "GBP",
                        Rate = (decimal) 10.1
                    },
                    new RateItem()
                    {
                        Amount = 10,
                        CurrencyCode = "GBPGFD",
                        Rate = (decimal) 10.1
                    }
                }
            };

            ICnbApiClient apiClientMock = Substitute.For<ICnbApiClient>();
            apiClientMock.GetExchangeRatesDaily(Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult<RatesResponse>(
                    ratesResponse
                ));

            var actor = new ExchangeRateFromApiRepository(apiClientMock, _logger, _configuration);

            var action = await actor.GetTodayCZKExchangeRatesDictionaryAsync();

            Assert.NotNull(action);

            Assert.True(action.Count == 2);

            var eurItem = action["EUR"];

            Assert.True(eurItem.Value.Value == (decimal)1.1 && eurItem.SourceCurrency.Code == "EUR");

            var gbpItem = action["GBP"];

            Assert.True(gbpItem.Value.Value == (decimal)1.01 && gbpItem.SourceCurrency.Code == "GBP");

            _logger.Received().Log(Arg.Is<LogLevel>(LogLevel.Error),
                                               Arg.Any<EventId>(),
                                               Arg.Any<Arg.AnyType>(),
                                               Arg.Any<Exception>(),
                                               Arg.Any<Func<Arg.AnyType, Exception?, string>>());
        }

    }
}
