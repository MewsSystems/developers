using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Contracts;

namespace ExchangeRateUpdater.CnbRates;

/// <summary>
/// Provides capability to retrieve exchange rates from CNB (Ceska Narodni Banka).
/// </summary>
public class CnbExchangeRatesProvider : IExchangeRateProvider
{
    private readonly ICnbClient cnbClient;

    public CnbExchangeRatesProvider(ICnbClient cnbClient)
    {
        this.cnbClient = cnbClient;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<ExchangeRate>> RetrieveExchangeRatesAsync(
        IReadOnlyCollection<Currency> currencies,
        CancellationToken cancellationToken)
    {
        var result = await cnbClient.RetrieveExchangeRatesAsync(cancellationToken);

        return result.Rates
            .Where(r => r.Amount != 0)
            .Select(r => new ExchangeRate(new Currency(r.CurrencyCode), Currency.Czk, r.Rate / r.Amount)).ToArray();
    }
}