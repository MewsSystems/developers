using ExchangeRateUpdater.Application.Components.Consumers;
using ExchangeRateUpdater.Application.Components.Queries;
using ExchangeRateUpdater.Application.Components.Responses;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ExchangeRateUpdater.Tests;

public class ConsoleApplicationTests
{
    private readonly List<ExchangeRate>  expectedRates = new List<ExchangeRate>
    {
        new(new Currency("USD"), new Currency("CZK"), 0.85m),
        new(new Currency("EUR"), new Currency("CZK"), 1m),
    };

    [Fact]
    public async Task ShouldSuccessfullySendRequestsToConsumer()
    {
        // Arrange
        var harness = new InMemoryTestHarness();
        var serviceProvider = BuildServiceProvider(new Mock<IExchangeRateProviderService>());
        var consumerHarness = harness.Consumer(() =>
        {
            var consumer = serviceProvider.GetRequiredService<GetExchangeRatesQueryConsumer>();
            return consumer;
        });

        // Act
        try
        {
            await harness.Start();
            await harness.InputQueueSendEndpoint.Send(new GetExchangeRatesQuery(new List<Currency> { new("USD"), new("EUR") }));

        }
        finally
        {
            await harness.Stop();
        }

        //Assert
        Assert.True(consumerHarness.Consumed.Select<GetExchangeRatesQuery>().Any());
    }

    [Fact]
    public async Task ShouldSuccessfullyRetrieveExchangeRates()
    {
        // Arrange
        var harness = new InMemoryTestHarness();
        var mockExchangeRateProviderService = new Mock<IExchangeRateProviderService>();
        mockExchangeRateProviderService.Setup(x => x.GetExchangeRates(It.IsAny<IEnumerable<Currency>>())).Returns(expectedRates);
        Response<GetExchangeRatesResponse>? response;

        var serviceProvider = BuildServiceProvider(mockExchangeRateProviderService);
        harness.Consumer(() =>
        {
            var consumer = serviceProvider.GetRequiredService<GetExchangeRatesQueryConsumer>();
            return consumer;
        });

        await harness.Start();
        // Act
        try
        {
            var reqClient = await harness.ConnectRequestClient<GetExchangeRatesQuery>();
            response = await reqClient.GetResponse<GetExchangeRatesResponse>(new GetExchangeRatesQuery(It.IsAny<IEnumerable<Currency>>()));
        }
        finally
        {
            await harness.Stop();
        }
        //Assert
        Assert.NotNull(response.Message.ExchangeRates);
        Assert.NotEmpty(response.Message.ExchangeRates);
        Assert.IsType<List<ExchangeRate>>(response.Message.ExchangeRates);
    }

    private static ServiceProvider BuildServiceProvider(IMock<IExchangeRateProviderService> exchangeRatesProviderServiceMock)
    {
        var services = new ServiceCollection();
        services.AddSingleton(exchangeRatesProviderServiceMock.Object);
        services.AddMediator(cfg => { cfg.AddConsumersFromNamespaceContaining<GetExchangeRatesQueryConsumer>(); });
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }



}