using ExchangeRateProvider.Application.Interfaces;
using ExchangeRateProvider.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider.Infrastructure.ExchangeRateProviders.Cnb;

/// <summary>
/// Exchange rate provider that retrieves exchange rate data from the Czech National Bank (CNB) API.
/// All exchange rates are provided with Czech Koruna (CZK) as the target currency.
/// </summary>
/// <param name="cnbApiOptions">Configuration options for the CNB API integration.</param>
/// <param name="httpClientFactory">Factory for creating HTTP clients to make API requests.</param>
/// <param name="logger">Logger for tracking API operations.</param>
public class CnbExchangeRateProvider(
    IOptions<CnbApiOptions> cnbApiOptions,
    IHttpClientFactory httpClientFactory,
    ILogger<CnbExchangeRateProvider> logger) : IExchangeRateProvider
{
    /// <summary>
    /// Retrieves the latest exchange rates from the Czech National Bank API.
    /// The rates are converted to have Czech Koruna (CZK) as the target currency and normalized per unit of source currency.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list
    /// of exchange rates with CZK as the target currency.</returns>
    public async Task<IReadOnlyList<ExchangeRate>> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        var httpClient = httpClientFactory.CreateClient(cnbApiOptions.Value.HttpClientName);

        var url = QueryHelpers.AddQueryString(
            cnbApiOptions.Value.DailyExchangeRatesEndpoint,
            "lang", "EN");

        logger.LogDebug("Making request to CNB API: {Url}", url);

        try
        {
            var rates = await httpClient.GetFromJsonAsync<CnbExchangeRates>(url, cancellationToken);

            if (rates is null)
            {
                logger.LogWarning("CNB API returned null or empty response");
                throw new InvalidOperationException("CNB API returned null or empty response");
            }

            logger.LogDebug("Successfully retrieved {RatesCount} rates from CNB API, valid for: {ValidFor}",
                rates.Rates.Count(),
                rates.Rates.FirstOrDefault()?.ValidFor.ToString());

            var target = new Currency("CZK");

            var result = rates.Rates
                .Select(r =>
                {
                    var source = new Currency(r.CurrencyCode);
                    var value = r.Rate / r.Amount;
                    var validFor = r.ValidFor;

                    return new ExchangeRate(source, target, value, validFor);
                })
                .ToList()
                .AsReadOnly();

            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while fetching exchange rates from CNB API: {Url}", url);
            throw;
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Request to CNB API was cancelled or timed out: {Url}", url);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while fetching exchange rates from CNB API: {Url}", url);
            throw;
        }
    }
}
