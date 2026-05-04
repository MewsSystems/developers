using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.Abstractions.Exceptions;
using ExchangeRateUpdater.Abstractions.Interfaces;
using ExchangeRateUpdater.Abstractions.Model;
using ExchangeRateUpdater.CnbClient.Dtos;
using ExchangeRateUpdater.CnbClient.Extensions;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.CnbClient.Implementation;

/// <summary>
/// HTTP-based implementation of the exchange rates client strategy.
/// </summary>
/// <param name="httpClientFactory"></param>
/// <param name="configuration"></param>
public class HttpExchangeRatesClientStrategy(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    : IExchangeRatesClientStrategy
{
    private readonly string url = configuration["CnbClient:Url"] ?? throw new ArgumentNullException("CnbClient:Url");

    /// <summary>
    /// Obtains the Exchange Rates from CNB.
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyList<CurrencyValue>> GetExchangeRates()
    {
        using var response = await httpClientFactory.CreateClient().GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync<ExchangeRatesResponseDto>(stream);

        return dto != null
            ? dto.Rates.Select(rate => rate.MapToCurrencyValue()).ToList()
            : throw new ExchangeRateNotFoundException("Exchange rates are not available from the source and no cached rates exist.");
    }
}