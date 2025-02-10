using ExchangeRateUpdater.Application.Exceptions;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Services;

/// <summary>
/// Service responsible for fetching exchange rates from the provider.
/// </summary>
public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateProvider _provider;
    private readonly ILogger<ExchangeRateService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeRateService"/> class.
    /// </summary>
    /// <param name="provider">The exchange rate provider.</param>
    /// <param name="logger">The logger instance.</param>
    public ExchangeRateService(IExchangeRateProvider provider, ILogger<ExchangeRateService> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ExchangeRateResponse> GetExchangeRatesAsync(
        DateTime date,
        IEnumerable<string>? currencies,
        CancellationToken cancellationToken)
    {
        var rates = await _provider.GetExchangeRatesAsync(date, cancellationToken);

        if (!rates.Any())
        {
            _logger.LogWarning("No exchange rates found for {Date}.", date);
            throw new NotFoundException("Exchange rate not found.");
        }

        _logger.LogInformation("Successfully fetched {Count} exchange rates for {Date}.", rates.Count(), date);

        // If no currencies are provided, return all exchange rates
        if (currencies == null || !currencies.Any())
        {
            return new ExchangeRateResponse(rates.ToList(), date.ToString("yyyy-MM-dd"), []);
        }

        // Convert requested currencies to a HashSet for efficient lookup
        HashSet<string> requestedCurrencies = new(currencies, StringComparer.OrdinalIgnoreCase);

        // Filter exchange rates for requested currencies
        var filteredRates = rates.Where(rate => requestedCurrencies.Contains(rate.TargetCurrency.Code)).ToList();

        // Identify missing currencies (requested but not found in API response)
        var missingCurrencies = requestedCurrencies
            .Where(currency => !filteredRates.Any(rate => rate.TargetCurrency.Code.Equals(currency, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        return new ExchangeRateResponse(filteredRates, date.ToString("yyyy-MM-dd"), missingCurrencies);
    }
}
