using ExchangeRateUpdater.Application.Components.Queries;
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
        try
        {
            var exchangeRates = await _exchangeRateProviderService.GetExchangeRates(context.Message.Currencies.Values);
            await context.RespondAsync(NonNullResponse<IEnumerable<ExchangeRate>>.Success(exchangeRates));
        }
        catch (Exception exception)
        {
            await context.RespondAsync(NonNullResponse<IEnumerable<ExchangeRate>>.Fail(new List<ExchangeRate>(),exception.Message));
        }
    }
}