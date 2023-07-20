using ExchangeRateUpdater.Application.Components.Queries;
using ExchangeRateUpdater.Application.Components.Responses;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using MassTransit;

namespace ExchangeRateUpdater.Application.Components.Consumers;

public class GetExchangeRatesQueryConsumer : IConsumer<GetExchangeRatesQuery>
{
    private readonly IExchangeRateProviderService _exchangeRateProviderService;

    public GetExchangeRatesQueryConsumer(IExchangeRateProviderService exchangeRateProviderService)
    {
        _exchangeRateProviderService = exchangeRateProviderService;
    }

    public async Task Consume(ConsumeContext<GetExchangeRatesQuery> context)
    {
        var exchangeRates = _exchangeRateProviderService.GetExchangeRates(context.Message.Currencies);
        await context.RespondAsync(new GetExchangeRatesResponse(exchangeRates));
    }
}