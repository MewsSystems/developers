using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Clients;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Extensions;
using ExchangeRateUpdater.Infrastructure.Interface.ExternalAPIs;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.Providers;

/// <inheritdoc />
internal class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateClient _exchangeRateClient;

    public ExchangeRateProvider(IExchangeRateClient exchangeRateClient)
    {
        _exchangeRateClient = exchangeRateClient;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        var allRates = await _exchangeRateClient.GetRates(cancellationToken);
        return allRates.FilterAndConvertRates(currencies);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(DateTime date, IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        var allRates = await _exchangeRateClient.GetRates(date, cancellationToken);
        return allRates.FilterAndConvertRates(currencies);
    }
}