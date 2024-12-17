using ExchangeRateUpdater.ApplicationServices.Interfaces;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.HttpClients;

namespace ExchangeRateUpdater.Infrastructure.Data.Repositories;

/// <summary>
/// Czech National Bank exchange rate repository.
/// </summary>
/// <seealso cref="ExchangeRateUpdater.ApplicationServices.Interfaces.IExchangeRateRepository" />
public class CnbExchangeRateRepository : IExchangeRateRepository
{
    //TODO: Implement a cache if needed.

    private readonly ICnbApiClient _cnbApiClient;

    public CnbExchangeRateRepository(ICnbApiClient cnbApiClient)
    {
        _cnbApiClient = cnbApiClient;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime date)
    {
        return (await _cnbApiClient.GetExchangeRatesAsync(date)).ToExchangeRates();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRate>> GetTodayExchangeRatesAsync()
    {
        return (await _cnbApiClient.GetTodayExchangeRatesAsync()).ToExchangeRates();
    }
}

public static class CnbExchangeRatesExtensions
{
    /// <summary>
    /// Converts CnbExchangeRates to ExchangeRate enumeration.
    /// </summary>
    /// <param name="cnbExchangeRates">The CNB exchange rates.</param>
    /// <returns>
    /// IEnumerable<paramref name="cnbExchangeRates" />
    /// </returns>
    public static IEnumerable<ExchangeRate> ToExchangeRates(this CnbExchangeRates cnbExchangeRates)
    {
        return cnbExchangeRates.Rates.Select(r => new ExchangeRate(new Currency(r.CurrencyCode), new Currency("CZK"), r.Rate / r.Amount));
    }
}