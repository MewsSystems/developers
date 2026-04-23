using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;

namespace Mews.ExchangeRate.Domain;
public class ExchangeRateProvider : IProvideExchangeRates
{
    private readonly ILogger<ExchangeRateProvider> _logger;
    private readonly IRetrieveExchangeRatesFromSource _exchangeRatesRetriever;

    public ExchangeRateProvider(ILogger<ExchangeRateProvider> logger,
        IRetrieveExchangeRatesFromSource exchangeRatesRetriever)
    {
        Guard.Against.Null(logger);
        Guard.Against.Null(exchangeRatesRetriever);

        _logger = logger;
        _exchangeRatesRetriever = exchangeRatesRetriever;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesForCurrenciesAsync(IEnumerable<Currency> currencies)
    {
        var currencyExchangeRates = new List<ExchangeRate>();

        var allExchangeRatesFromSource = await _exchangeRatesRetriever.GetAllExchangeRatesAsync();
        var allExchangeRatesGroupedByCurrency = GroupExchangeRatesByCurrency(allExchangeRatesFromSource);

        foreach (var currency in currencies)
        {
            _logger.LogInformation("Retrieving exchange rates for {currency}",
                currency);

            if (allExchangeRatesGroupedByCurrency.TryGetValue(currency.Code, out var exchangeRatesForCurrency))
            {
                currencyExchangeRates.AddRange(exchangeRatesForCurrency);
            }
        }

        return currencyExchangeRates;
    }

    private Dictionary<string, IList<ExchangeRate>> GroupExchangeRatesByCurrency(IEnumerable<ExchangeRate> exchangeRates)
    {
        var exchangeRatesGroupedByCurrency = new Dictionary<string, IList<ExchangeRate>>();

        foreach (var exchangeRate in exchangeRates)
        {
            if (!exchangeRatesGroupedByCurrency.TryGetValue(exchangeRate.TargetCurrency.Code, out var targetExchangeRatesList))
            {
                targetExchangeRatesList = new List<ExchangeRate>();
                exchangeRatesGroupedByCurrency.Add(exchangeRate.TargetCurrency.Code, targetExchangeRatesList);
            }

            targetExchangeRatesList.Add(exchangeRate);

            if (!exchangeRatesGroupedByCurrency.TryGetValue(exchangeRate.SourceCurrency.Code, out var sourceExchangeRatesList))
            {
                sourceExchangeRatesList = new List<ExchangeRate>();
                exchangeRatesGroupedByCurrency.Add(exchangeRate.SourceCurrency.Code, sourceExchangeRatesList);
            }

            sourceExchangeRatesList.Add(exchangeRate);
        }

        return exchangeRatesGroupedByCurrency;
    }
}
