using CurrencyExchange.Clients;
using CurrencyExchange.Model;

namespace CurrencyExchange.Services;

public class ExchangeRateProvider(ICurrencyExchangeClient currencyExchangeClient) : IExchangeRateProvider
{
    private readonly ICurrencyExchangeClient _currencyExchangeClient = currencyExchangeClient;

    private readonly Currency _baseCurrency = new("CZK");

    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        var retrievedRates = await _currencyExchangeClient.GetDailyRates(cancellationToken);

        var desiredCurrencyCodes = currencies.Select(c => c.Code);

        var availableRates = retrievedRates.Rates.IntersectBy(desiredCurrencyCodes, r => r.CurrencyCode);
        LogMissingRates(desiredCurrencyCodes, retrievedRates.Rates.Select(r => r.CurrencyCode));

        return availableRates.Select(r => new ExchangeRate
        {
            SourceCurrency = _baseCurrency,
            TargetCurrency = new Currency(r.CurrencyCode),
            Value = r.Rate / r.Amount,
            ValidFor = r.ValidFor
        });
    }

    private static void LogMissingRates(IEnumerable<string> desiredCurrencyCodes, IEnumerable<string> retrievedCurrencyCodes)
    {
        var missingCurrencyCodes = desiredCurrencyCodes.Except(retrievedCurrencyCodes);

        if (missingCurrencyCodes.Any())
        {
            Console.WriteLine($"Rates for following requested currencies were not available: {string.Join(", ", missingCurrencyCodes)}");
        }
    }
}
