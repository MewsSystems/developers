using ExchangeRateUpdater.Application.Components.Consumers;
using ExchangeRateUpdater.Application.Components.Queries;
using ExchangeRateUpdater.Application.Components.Responses;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using MassTransit;
using Moq;

namespace ExchangeRateUpdater.Tests;

public class GetExchangeRatesQueryConsumerTests
{
    private readonly Mock<IExchangeRateProviderService> _mockExchangeRateProviderService;
    private readonly Mock<ConsumeContext<GetExchangeRatesQuery>> _mockConsumeContext;
    private readonly List<Currency> _currencies = new() { new Currency("USD"), new Currency("EUR") };

    public GetExchangeRatesQueryConsumerTests()
    {
        _mockExchangeRateProviderService = new Mock<IExchangeRateProviderService>();
        _mockConsumeContext = new Mock<ConsumeContext<GetExchangeRatesQuery>>();
    }

    [Fact]
    public async Task Consume_Should_ReturnExchangeRates_When_Successful()
    {
        // Arrange
        var query = new GetExchangeRatesQuery(NonEmptyList<Currency>.Create(_currencies));
        var expectedRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 0.85m),
            new(new Currency("EUR"), new Currency("CZK"), 1m),
        };

        _mockExchangeRateProviderService.Setup(x => x.GetExchangeRates(query.Currencies.Values)).ReturnsAsync(expectedRates);
        _mockConsumeContext.Setup(x => x.Message).Returns(query);

        var consumer = new GetExchangeRatesQueryConsumer(_mockExchangeRateProviderService.Object);

        // Act
        await consumer.Consume(_mockConsumeContext.Object);


        // Assert
        _mockConsumeContext.Verify(x => x.RespondAsync(It.Is<GetExchangeRatesResponse>(r =>
                r.ExchangeRates == expectedRates)),
            Times.Once);
    }

}