using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    public const string CZECH_KORUNA_CODE = "CZK";
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRateProvider> _logger;
    private readonly ExchangeRateProviderOptions _exchangeRateProviderOptions;
    private readonly CzkExchangeRateFactory _factory;

    public ExchangeRateProvider(HttpClient httpClient,
                                ILogger<ExchangeRateProvider> logger,
                                IOptions<ExchangeRateProviderOptions> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _exchangeRateProviderOptions = options.Value;
        _factory = new CzkExchangeRateFactory();
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        if (currencies is null || !currencies.Any())
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        ExchangeRateResponsePayload response = null;
        try
        {
            response = await _httpClient.GetFromJsonAsync<ExchangeRateResponsePayload>(_exchangeRateProviderOptions.ApiRequestUri);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error thrown calling exchange rate api");

            throw;
        }

        if (response is null || response.Rates is null || !response.Rates.Any())
        {
            _logger.LogWarning("Exchange rate response contained no rates");

            return Enumerable.Empty<ExchangeRate>();
        }

        var returnRates = new List<ExchangeRate>();
        var payloadRates = response.Rates;
        foreach (var currency in currencies)
        {
            var rate = payloadRates.FirstOrDefault(r => r.CurrencyCode == currency.Code);
            if (rate is not null)
            {
                returnRates.Add(_factory.Create(rate));
            }
        }

        return returnRates.ToArray();
    }

    protected record ExchangeRateResponsePayload(ExchangeRateResponse[] Rates);

    public record ExchangeRateResponse(string CurrencyCode, decimal Rate, int Amount);
}

