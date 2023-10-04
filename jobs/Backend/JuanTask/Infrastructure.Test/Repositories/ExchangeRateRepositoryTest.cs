using Infrastructure.Clients.Cnb;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Text;

namespace Infrastructure.Test.Repositories
{
    public class ExchangeRateRepositoryTest
    {

        private readonly ILogger<ExchangeRateRepository> _logger;

        private readonly IOptions<ExchangeRateRepositoryConfiguration> _configuration;

        public ExchangeRateRepositoryTest() 
        {
            _logger = Substitute.For<ILogger<ExchangeRateRepository>>();
            _configuration = Substitute.For<IOptions<ExchangeRateRepositoryConfiguration>>();
            _configuration.Value.Returns(new ExchangeRateRepositoryConfiguration()
            {
                AmountTitle = "amount",
                CodeTitle = "code",
                RateTitle= "rate",
                TargetCurrency = "CZK"
            });
        }

        [Fact]
        public async Task ExchangeRateRepository_GetTodayCZKExchangeRatesDictionaryAsync_CsvIsNull()
        {

            string csvResponse = null;

            ICnbzClient cnbzClientMock = Substitute.For<ICnbzClient>();
            cnbzClientMock.GetExchangeRateAmountCsvAsync(Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult<string>(
                    csvResponse
                ));

            var actor = new ExchangeRateRepository(cnbzClientMock, _configuration, _logger);

            var action = await actor.GetTodayCZKExchangeRatesDictionaryAsync();

            Assert.NotNull(action);
            Assert.Empty(action);

        }

        [Fact]
        public async Task ExchangeRateRepository_GetTodayCZKExchangeRatesDictionaryAsync_CsvIsEmpty()
        {

            string csvResponse = string.Empty;

            ICnbzClient cnbzClientMock = Substitute.For<ICnbzClient>();
            cnbzClientMock.GetExchangeRateAmountCsvAsync(Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult<string>(
                    csvResponse
                ));

            var actor = new ExchangeRateRepository(cnbzClientMock, _configuration, _logger);

            var action = await actor.GetTodayCZKExchangeRatesDictionaryAsync();

            Assert.NotNull(action);
            Assert.Empty(action);

        }

        [Fact]
        public async Task ExchangeRateRepository_GetTodayCZKExchangeRatesDictionaryAsync_MultipleInvalidLinesWithOneCorrectLine()
        {

            StringBuilder csvResponse = new StringBuilder();
            csvResponse.Append("dsafsa fsda fsaf sad\n");
            csvResponse.Append("Country|Currency|Amount|Code|Rate\n");
            csvResponse.Append("Country|Currency|Amount|Code|Rate\n");
            csvResponse.Append("dsafsa | fsda | fsaf | sad\n");
            csvResponse.Append("peso|1|MXN|1.336\n");
            csvResponse.Append("Switzerland|franc|1|CHF|25.275\n");
            csvResponse.Append("dsafsa | fsda | fsaf | sad\n");

            ICnbzClient cnbzClientMock = Substitute.For<ICnbzClient>();
            cnbzClientMock.GetExchangeRateAmountCsvAsync(Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult<string>(
                    csvResponse.ToString()
                ));

            var actor = new ExchangeRateRepository(cnbzClientMock, _configuration, _logger);

            var action = await actor.GetTodayCZKExchangeRatesDictionaryAsync();

            Assert.NotNull(action);
            Assert.True(action.Count == 1);
            Assert.True(action["CHF"].SourceCurrency.Code == "CHF");
            Assert.True(action["CHF"].TargetCurrency.Code == "CZK");
            Assert.True(action["CHF"].Value.Value == (decimal)25.275);
        }

        [Fact]
        public async Task ExchangeRateRepository_GetTodayCZKExchangeRatesDictionaryAsync_ValueDecimalParseError()
        {

            StringBuilder csvResponse = new StringBuilder();
            csvResponse.Append("Country|Currency|Amount|Code|Rate\n");
            csvResponse.Append("Mexico|peso|1|MXN|1.336dsafsa\n");
            csvResponse.Append("Switzerland|franc|1|CHF|25.275\n");

            ICnbzClient cnbzClientMock = Substitute.For<ICnbzClient>();
            cnbzClientMock.GetExchangeRateAmountCsvAsync(Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult<string>(
                    csvResponse.ToString()
                ));

            var actor = new ExchangeRateRepository(cnbzClientMock, _configuration, _logger);

            var action = await actor.GetTodayCZKExchangeRatesDictionaryAsync();

            Assert.NotNull(action);
            Assert.True(action.Count == 1);
            Assert.True(action["CHF"].SourceCurrency.Code == "CHF");
            Assert.True(action["CHF"].TargetCurrency.Code == "CZK");
            Assert.True(action["CHF"].Value.Value == (decimal)25.275);

            _logger.Received().Log(Arg.Is<LogLevel>(LogLevel.Error),
                                   Arg.Any<EventId>(),
                                   Arg.Any<Arg.AnyType>(),
                                   Arg.Any<Exception>(), 
                                   Arg.Any<Func<Arg.AnyType, Exception?, string>>());

        }

        [Fact]
        public async Task ExchangeRateRepository_GetTodayCZKExchangeRatesDictionaryAsync_ValueIntParseError()
        {

            StringBuilder csvResponse = new StringBuilder();
            csvResponse.Append("Country|Currency|Amount|Code|Rate\n");
            csvResponse.Append("Mexico|peso|1sadf|MXN|1.336\n");
            csvResponse.Append("Switzerland|franc|1|CHF|25.275\n");

            ICnbzClient cnbzClientMock = Substitute.For<ICnbzClient>();
            cnbzClientMock.GetExchangeRateAmountCsvAsync(Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult<string>(
                    csvResponse.ToString()
                ));

            var actor = new ExchangeRateRepository(cnbzClientMock, _configuration, _logger);

            var action = await actor.GetTodayCZKExchangeRatesDictionaryAsync();

            Assert.NotNull(action);
            Assert.True(action.Count == 1);
            Assert.True(action["CHF"].SourceCurrency.Code == "CHF");
            Assert.True(action["CHF"].TargetCurrency.Code == "CZK");
            Assert.True(action["CHF"].Value.Value == (decimal)25.275);

            _logger.Received().Log(Arg.Is<LogLevel>(LogLevel.Error),
                                   Arg.Any<EventId>(),
                                   Arg.Any<Arg.AnyType>(),
                                   Arg.Any<Exception>(),
                                   Arg.Any<Func<Arg.AnyType, Exception?, string>>());

        }

        [Fact]
        public async Task ExchangeRateRepository_GetTodayCZKExchangeRatesDictionaryAsync_ValueCurrencyError()
        {

            StringBuilder csvResponse = new StringBuilder();
            csvResponse.Append("Country|Currency|Amount|Code|Rate\n");
            csvResponse.Append("Mexico|peso|1|MXDSAFSN|1.336\n");
            csvResponse.Append("Switzerland|franc|1|CHF|25.275\n");

            ICnbzClient cnbzClientMock = Substitute.For<ICnbzClient>();
            cnbzClientMock.GetExchangeRateAmountCsvAsync(Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult<string>(
                    csvResponse.ToString()
                ));

            var actor = new ExchangeRateRepository(cnbzClientMock, _configuration, _logger);

            var action = await actor.GetTodayCZKExchangeRatesDictionaryAsync();

            Assert.NotNull(action);
            Assert.True(action.Count == 1);
            Assert.True(action["CHF"].SourceCurrency.Code == "CHF");
            Assert.True(action["CHF"].TargetCurrency.Code == "CZK");
            Assert.True(action["CHF"].Value.Value == (decimal)25.275);

            _logger.Received().Log(Arg.Is<LogLevel>(LogLevel.Error),
                                   Arg.Any<EventId>(),
                                   Arg.Any<Arg.AnyType>(),
                                   Arg.Any<Exception>(),
                                   Arg.Any<Func<Arg.AnyType, Exception?, string>>());

        }

        [Fact]
        public async Task ExchangeRateRepository_GetTodayCZKExchangeRatesDictionaryAsync_CsvWithoutTitle()
        {

            StringBuilder csvResponse = new StringBuilder();
            csvResponse.Append("Mexico|peso|1|MXD|1.336\n");
            csvResponse.Append("Switzerland|franc|1|CHF|25.275\n");

            ICnbzClient cnbzClientMock = Substitute.For<ICnbzClient>();
            cnbzClientMock.GetExchangeRateAmountCsvAsync(Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult<string>(
                    csvResponse.ToString()
                ));

            var actor = new ExchangeRateRepository(cnbzClientMock, _configuration, _logger);

            var action = await actor.GetTodayCZKExchangeRatesDictionaryAsync();

            Assert.NotNull(action);
            Assert.True(action.Count == 0);

        }


    }
}
