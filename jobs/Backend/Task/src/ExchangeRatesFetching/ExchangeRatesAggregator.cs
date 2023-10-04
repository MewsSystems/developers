using ExchangeRatesUpdater.Common;
using Microsoft.Extensions.Logging;

namespace ExchangeRatesFetching;

public interface IExchangeRatesAggregator
{
    Task<IReadOnlyList<ExchangeRate>> GetExchangeRates(IDictionary<string, IEnumerable<string>> bankCurrenciesDictionary);
}

public class ExchangeRatesAggregator : IExchangeRatesAggregator
{
    private readonly ILogger<ExchangeRatesAggregator> logger;
    private readonly IExchangeRatesProviderFactory exchangeRatesProviderFactory;

    public ExchangeRatesAggregator(ILogger<ExchangeRatesAggregator> logger, IExchangeRatesProviderFactory exchangeRatesProviderFactory)
    {
        this.logger = logger;
        this.exchangeRatesProviderFactory = exchangeRatesProviderFactory;
    }

    public async Task<IReadOnlyList<ExchangeRate>> GetExchangeRates(IDictionary<string, IEnumerable<string>> bankCurrenciesDictionary)
    {
        List<ExchangeRate> exchangeRates = new();

        foreach (var keyValuePair in bankCurrenciesDictionary) {
            if (!keyValuePair.Value.Any()) continue;

            IExchangeRatesProvider? provider = exchangeRatesProviderFactory.GetProvider(keyValuePair.Key);

            if (provider == null) {
                logger.LogWarning("Attempted to use invalid provider name ({name})", keyValuePair.Key);
                continue;
            }

            IEnumerable<ExchangeRate> rates = await provider.GetRatesForCurrenciesAsync(keyValuePair.Value);
            exchangeRates.AddRange(rates);
        }

        return exchangeRates;
    }
}
