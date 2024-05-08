using ExchangeRateUpdater.Application.Handlers.QueryHandlers.Abstract;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Application.Handlers.QueryHandlers.GetSupportedExchangeRates;

public class GetSupportedExchangeRatesQueryHandler(
    IExternalExchangeRatesProvider externalExchangeRatesProvider,
    IOptions<ExchangeRatesOptions> exchangeRatesOptions)
    : IAsyncQueryHandler<IEnumerable<ExchangeRate>>
{
    private readonly IExternalExchangeRatesProvider _externalExchangeRatesProvider = externalExchangeRatesProvider;
    private readonly ExchangeRatesOptions _exchangeRatesOptions = exchangeRatesOptions.Value;

    public async Task<IEnumerable<ExchangeRate>> HandleAsync()
    {
        var exchangeRates = await _externalExchangeRatesProvider.ProvideAsync();

        return exchangeRates.Where(er => _exchangeRatesOptions.SupportedCurrencies.ToList().Contains(er.SourceCurrency.Code.ToUpper()));
    }
}
