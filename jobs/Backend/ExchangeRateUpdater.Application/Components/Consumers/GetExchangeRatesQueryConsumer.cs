using ExchangeRateUpdater.Application.Components.Queries;
using ExchangeRateUpdater.Application.Components.Responses;
using ExchangeRateUpdater.Application.Services;
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
        var exchangeRates = await _exchangeRateProviderService.GetExchangeRates(context.Message.Currencies.Values);
        await context.RespondAsync(new GetExchangeRatesResponse(exchangeRates));
    }
}