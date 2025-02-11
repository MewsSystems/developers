using ExchangeRateUpdater.Application.Exceptions;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.Caching;
using ExchangeRateUpdater.Infrastructure.Interfaces;
using ExchangeRateUpdater.Infrastructure.Mappers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Providers;

/// <summary>
/// Provides exchange rate data from the Czech National Bank (CNB).
/// </summary>
public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICnbApiClient _apiClient;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CzechNationalBankExchangeRateProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CzechNationalBankExchangeRateProvider"/> class.
    /// </summary>
    /// <param name="apiClient">The client used to communicate with the CNB API.</param>
    /// <param name="cacheService">The caching service to optimize API calls.</param>
    /// <param name="logger">The logger instance.</param>
    public CzechNationalBankExchangeRateProvider(
        ICnbApiClient apiClient,
        ICacheService cacheService,
        ILogger<CzechNationalBankExchangeRateProvider> logger)
    {
        _apiClient = apiClient;
        _cacheService = cacheService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = CacheKeyGenerator.Generate<ExchangeRate>(date.ToString("yyyy-MM-dd"));

        var exchangeRates = await _cacheService.GetOrCreateAsync(
            cacheKey,
            async () => await LoadExchangeRates(date, cancellationToken));

        return exchangeRates!;
    }

    private async Task<IEnumerable<ExchangeRate>> LoadExchangeRates(DateTime date, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching exchange rates from CNB API for {Date}.", date.ToString("yyyy-MM-dd"));

        var response = await _apiClient.GetExchangeRatesAsync(date, cancellationToken);
        var exchangeRates = CnbExchangeRateMapper.MapToDomainModel(response);

        if (!exchangeRates.Any())
        {
            _logger.LogWarning("No exchange rates found for {Date}.", date);
            throw new NotFoundException("Exchange rate not found.");
        }

        _logger.LogInformation("Successfully retrieved {Count} exchange rates for {Date}.", exchangeRates.Count(), date);
        return exchangeRates;
    }
}
