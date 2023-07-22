using ExchangeRateUpdater.Application.Components.Consumers;
using ExchangeRateUpdater.Application.Components.Queries;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ExchangeRateUpdater.Tests;

public class ConsoleApplicationTests
{

    [Fact]
    public async Task ShouldSuccessfullySendRequestsToConsumer()
    {
        // Arrange
        var harness = new InMemoryTestHarness();
        var serviceProvider = BuildServiceProvider(new Mock<IExchangeRateProviderService>());
        var consumerHarness = harness.Consumer(() =>
        {
            var consumer = serviceProvider.GetRequiredService<GetExchangeRatesForCurrenciesQueryConsumer>();
            return consumer;
        });

        // Act
        try
        {
            await harness.Start();
            await harness.InputQueueSendEndpoint.Send(new GetExchangeRatesForCurrenciesQuery(It.IsAny<NonEmptyList<ValidCurrency>>()));

        }
        finally
        {
            await harness.Stop();
        }

        //Assert
        Assert.True(consumerHarness.Consumed.Select<GetExchangeRatesForCurrenciesQuery>().Any());
    }

    private static ServiceProvider BuildServiceProvider(IMock<IExchangeRateProviderService> exchangeRatesProviderServiceMock)
    {
        var services = new ServiceCollection();
        services.AddSingleton(exchangeRatesProviderServiceMock.Object);
        services.AddMediator(cfg => { cfg.AddConsumersFromNamespaceContaining<GetExchangeRatesForCurrenciesQueryConsumer>(); });
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}