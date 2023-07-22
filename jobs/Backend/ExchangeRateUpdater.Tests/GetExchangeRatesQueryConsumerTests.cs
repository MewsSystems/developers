using ExchangeRateUpdater.Application.Components.Consumers;
using ExchangeRateUpdater.Application.Components.Queries;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using MassTransit;
using Moq;
using Serilog;

namespace ExchangeRateUpdater.Tests;

public class GetExchangeRatesQueryConsumerTests
{
    private readonly Mock<IExchangeRateProviderService> _mockExchangeRateProviderService;
    private readonly Mock<ConsumeContext<GetExchangeRatesForCurrenciesQuery>> _mockConsumeContext;
    private readonly Mock<ILogger> _mockLogger;
    private readonly List<ValidCurrency> _currencies = new() { ValidCurrency.CreateUnsafe("USD"), ValidCurrency.CreateUnsafe("EUR") };

    public GetExchangeRatesQueryConsumerTests()
    {
        _mockExchangeRateProviderService = new Mock<IExchangeRateProviderService>();
        _mockConsumeContext = new Mock<ConsumeContext<GetExchangeRatesForCurrenciesQuery>>();
        _mockLogger = new Mock<ILogger>();
    }

    [Fact]
    public async Task Consume_Should_ReturnExchangeRates_When_Successful()
    {
        // Arrange
        var query = new GetExchangeRatesForCurrenciesQuery(NonEmptyList<ValidCurrency>.CreateUnsafe(_currencies));
        var expectedRates = NonNullResponse<Dictionary<string, ExchangeRate>>.Success(new Dictionary<string, ExchangeRate>
        {
            {"USD",new ExchangeRate(new Currency("USD"),new Currency("CZK"),1m)},
            {"EUR",new ExchangeRate(new Currency("EUR"),new Currency("CZK"),1m)},
        });

        _mockExchangeRateProviderService.Setup(x => x.GetExchangeRates()).ReturnsAsync(expectedRates);
        _mockConsumeContext.Setup(x => x.Message).Returns(query);

        var consumer = new GetExchangeRatesForCurrenciesQueryConsumer(_mockExchangeRateProviderService.Object, _mockLogger.Object);

        // Act
        await consumer.Consume(_mockConsumeContext.Object);


        // Assert
        _mockConsumeContext.Verify(x => x.RespondAsync(It.Is<NonNullResponse<IEnumerable<ExchangeRate>>>(r =>
                r.Content == expectedRates)),
            Times.Once);
    }

}