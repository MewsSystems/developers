using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mews.Integrations.Cnb.Clients;
using Mews.Integrations.Cnb.Contracts.Models;
using Mews.Integrations.Cnb.Contracts.Services;
using Mews.Integrations.Cnb.Models;
using Microsoft.Extensions.Logging;

namespace Mews.Integrations.Cnb.Services;

public class ExchangeRateProvider(ICnbClient cnbClient, ILogger<ExchangeRateProvider> logger) : IExchangeRateProvider
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        DateTimeOffset date,
        CancellationToken cancellationToken)
    {
        var exchangeRateResponse = await cnbClient.GetDailyExchangeRatesAsync(date, cancellationToken);
        var currencyCodes = currencies.Select(c => c.Code).ToList();

        var targetRates = exchangeRateResponse.Rates
            .Where(r => currencyCodes.Contains(r.CurrencyCode))
            .ToArray();
        
        LogMissingCurrencies(currencyCodes, targetRates);

        return targetRates
            .Select(r => new ExchangeRate
            {
                SourceCurrency = new Currency(r.CurrencyCode),
                TargetCurrency = new Currency("CZK"),
                Value = r.Rate / r.Amount,
                ValidFor = GetValidFor(r.ValidFor),
            })
            .ToList();
    }

    private void LogMissingCurrencies(List<string> currencyCodes, CnbClientExchangeRateResponseItem[] targetRates)
    {
        var missingCurrencies = currencyCodes.Except(targetRates.Select(r => r.CurrencyCode)).ToList();
        var formattedMissingCurrencies = string.Join(", ", missingCurrencies);
        logger.LogWarning("Missing exchange rates for currencies: {formattedMissingCurrencies}", formattedMissingCurrencies);
    }

    private static DateTimeOffset GetValidFor(string date)
    {
        return DateTimeOffset.Parse(date);
    }
}
