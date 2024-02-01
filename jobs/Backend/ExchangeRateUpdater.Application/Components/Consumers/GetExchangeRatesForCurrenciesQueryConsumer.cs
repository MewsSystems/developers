using ExchangeRateUpdater.Application.Components.Queries;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using MassTransit;
using Serilog;

namespace ExchangeRateUpdater.Application.Components.Consumers;

public class GetExchangeRatesForCurrenciesQueryConsumer : IConsumer<GetExchangeRatesForCurrenciesQuery>
{
    private readonly IExchangeRateProviderService _exchangeRateProviderService;
    private readonly ILogger _logger;

    public GetExchangeRatesForCurrenciesQueryConsumer(IExchangeRateProviderService exchangeRateProviderService, ILogger logger)
    {
        _exchangeRateProviderService = exchangeRateProviderService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<GetExchangeRatesForCurrenciesQuery> context)
    {
        try
        {
            var providerResponse = await _exchangeRateProviderService.GetExchangeRates();
            if (providerResponse.IsSuccess)
            {
                var exchangeRates = MatchCurrenciesWithExchanges(context.Message.Currencies.Values, providerResponse.Content);
                await context.RespondAsync(NonNullResponse<IEnumerable<ExchangeRate>>.Success(exchangeRates));
            }
            else
            {
                _logger.Error("Exchange rate provider has responded with fail: {@response}",providerResponse);
                await context.RespondAsync(NonNullResponse<IEnumerable<ExchangeRate>>.Fail(new List<ExchangeRate>(), "It's not you, it's us, we are having some troubles retrieving exchanges please come back later"));
            }
        }
        catch (Exception exception)
        {
            _logger.Error(exception, "Error while retrieving exchanges");
            await context.RespondAsync(NonNullResponse<IEnumerable<ExchangeRate>>.Fail(new List<ExchangeRate>(),"It's not you, it's us, we are having some troubles retrieving exchanges please come back later"));
        }
    }

    private static IEnumerable<ExchangeRate> MatchCurrenciesWithExchanges(IEnumerable<ValidCurrency> currenciesRequested, IReadOnlyDictionary<string, ExchangeRate> exchangeRatesFound)
    {
        var exchangeRatesMatched = new List<ExchangeRate>();
        foreach (var currency in currenciesRequested)
        {
            if (exchangeRatesFound.TryGetValue(currency.ToString() ?? "", out var value))
            {
                exchangeRatesMatched.Add(value);
            }
        }
        return exchangeRatesMatched;
    }
}