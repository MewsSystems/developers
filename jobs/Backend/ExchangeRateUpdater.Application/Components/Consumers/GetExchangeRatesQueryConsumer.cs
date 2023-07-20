using ExchangeRateUpdater.Application.Components.Queries;
using MassTransit;

namespace ExchangeRateUpdater.Application.Components.Consumers;

public class GetExchangeRatesQueryConsumer : IConsumer<GetExchangeRatesQuery>
{
    public Task Consume(ConsumeContext<GetExchangeRatesQuery> context)
    {
        throw new NotImplementedException();
    }
}