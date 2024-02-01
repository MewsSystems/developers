using ExchangeRateUpdater.Application.Components.Consumers;
using ExchangeRateUpdater.Application.Components.Queries;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using MassTransit;
using Moq;
using Serilog;

namespace ExchangeRateUpdater.Tests;

public class GetExchangeRatesForCurrenciesQueryConsumerTests
{
    private readonly Mock<IExchangeRateProviderService> _mockExchangeRateProviderService;
    private readonly Mock<ConsumeContext<GetExchangeRatesForCurrenciesQuery>> _mockConsumeContext;
    private readonly Mock<ILogger> _mockLogger;
    private readonly List<ValidCurrency> _currencies = new() { ValidCurrency.CreateUnsafe("USD"), ValidCurrency.CreateUnsafe("EUR") };

    public GetExchangeRatesForCurrenciesQueryConsumerTests()
    {
        _mockExchangeRateProviderService = new Mock<IExchangeRateProviderService>();
        _mockConsumeContext = new Mock<ConsumeContext<GetExchangeRatesForCurrenciesQuery>>();
        _mockLogger = new Mock<ILogger>();
    }

    [Fact]
    public async Task Consume_Should_ReturnSuccessResult_WithExchangeRates_On_ProviderSuccess()
    {
        // Arrange
        var query = new GetExchangeRatesForCurrenciesQuery(NonEmptyList<ValidCurrency>.CreateUnsafe(_currencies));
        var providerRates = NonNullResponse<Dictionary<string, ExchangeRate>>.Success(new Dictionary<string, ExchangeRate>
        {
            {"USD",new ExchangeRate(new Currency("USD"),new Currency("CZK"),1m)},
            {"EUR",new ExchangeRate(new Currency("EUR"),new Currency("CZK"),1m)},
        });

        _mockExchangeRateProviderService.Setup(x => x.GetExchangeRates()).ReturnsAsync(providerRates);
        _mockConsumeContext.Setup(x => x.Message).Returns(query);

        var consumer = new GetExchangeRatesForCurrenciesQueryConsumer(_mockExchangeRateProviderService.Object, _mockLogger.Object);

        // Act
        await consumer.Consume(_mockConsumeContext.Object);


        // Assert
        _mockConsumeContext.Verify(x => x.RespondAsync(It.Is<NonNullResponse<IEnumerable<ExchangeRate>>>(r =>
                r.Content.Count() == 2 &&
                r.IsSuccess)),
            Times.Once);
    }    
    
    [Fact]
    public async Task Consume_Should_ReturnSuccessResult_WithEmptyContent_On_ProviderReturningEmptyResult()
    {
        // Arrange
        var query = new GetExchangeRatesForCurrenciesQuery(NonEmptyList<ValidCurrency>.CreateUnsafe(_currencies));
        var providerRates = NonNullResponse<Dictionary<string, ExchangeRate>>.Success(new Dictionary<string, ExchangeRate>());

        _mockExchangeRateProviderService.Setup(x => x.GetExchangeRates()).ReturnsAsync(providerRates);
        _mockConsumeContext.Setup(x => x.Message).Returns(query);

        var consumer = new GetExchangeRatesForCurrenciesQueryConsumer(_mockExchangeRateProviderService.Object, _mockLogger.Object);

        // Act
        await consumer.Consume(_mockConsumeContext.Object);


        // Assert
        _mockConsumeContext.Verify(x => x.RespondAsync(It.Is<NonNullResponse<IEnumerable<ExchangeRate>>>(r =>
                !r.Content.Any() &&
                r.IsSuccess)),
            Times.Once);
    }   
    
    [Fact]
    public async Task Consume_Should_ReturnSuccessResult_WithEmptyContent_On_NoCurrencyMatched()
    {
        // Arrange
        var query = new GetExchangeRatesForCurrenciesQuery(NonEmptyList<ValidCurrency>.CreateUnsafe(_currencies));
        var providerRates = NonNullResponse<Dictionary<string, ExchangeRate>>.Success(new Dictionary<string, ExchangeRate>
        {
            {"JPY",new ExchangeRate(new Currency("JPY"),new Currency("CZK"),1m)},
            {"CZK",new ExchangeRate(new Currency("CZK"),new Currency("CZK"),1m)},
        });

        _mockExchangeRateProviderService.Setup(x => x.GetExchangeRates()).ReturnsAsync(providerRates);
        _mockConsumeContext.Setup(x => x.Message).Returns(query);

        var consumer = new GetExchangeRatesForCurrenciesQueryConsumer(_mockExchangeRateProviderService.Object, _mockLogger.Object);

        // Act
        await consumer.Consume(_mockConsumeContext.Object);


        // Assert
        _mockConsumeContext.Verify(x => x.RespondAsync(It.Is<NonNullResponse<IEnumerable<ExchangeRate>>>(r =>
                !r.Content.Any() &&
                r.IsSuccess)),
            Times.Once);
    }  
    
    [Fact]
    public async Task Consume_Should_ReturnFailResult_WithNonNullContent_And_ErrorMessage_On_ProviderFail()
    {
        // Arrange
        var query = new GetExchangeRatesForCurrenciesQuery(NonEmptyList<ValidCurrency>.CreateUnsafe(_currencies));
        var expectedResult = NonNullResponse<Dictionary<string, ExchangeRate>>.Fail(new Dictionary<string, ExchangeRate>(), "test");

        _mockExchangeRateProviderService.Setup(x => x.GetExchangeRates()).ReturnsAsync(expectedResult);
        _mockConsumeContext.Setup(x => x.Message).Returns(query);

        var consumer = new GetExchangeRatesForCurrenciesQueryConsumer(_mockExchangeRateProviderService.Object, _mockLogger.Object);

        // Act
        await consumer.Consume(_mockConsumeContext.Object);


        // Assert
        _mockConsumeContext.Verify(x => x.RespondAsync(It.Is<NonNullResponse<IEnumerable<ExchangeRate>>>(r =>
                !r.IsSuccess &&
                r.Errors.Count>0 &&
                r.Content != null
                )),
            Times.Once);
    }    
    
    [Fact]
    public async Task Consume_Should_ReturnFailResult_WithNonNullContent_And_ErrorMessage_When_ExceptionIsThrown()
    {
        // Arrange
        var query = new GetExchangeRatesForCurrenciesQuery(NonEmptyList<ValidCurrency>.CreateUnsafe(_currencies));

        _mockExchangeRateProviderService.Setup(x => x.GetExchangeRates()).Throws(new Exception("test"));
        _mockConsumeContext.Setup(x => x.Message).Returns(query);

        var consumer = new GetExchangeRatesForCurrenciesQueryConsumer(_mockExchangeRateProviderService.Object, _mockLogger.Object);

        // Act
        await consumer.Consume(_mockConsumeContext.Object);


        // Assert
        _mockConsumeContext.Verify(x => x.RespondAsync(It.Is<NonNullResponse<IEnumerable<ExchangeRate>>>(r =>
                !r.IsSuccess &&
                r.Errors.Count>0 &&
                r.Content != null
                )),
            Times.Once);
    }

}