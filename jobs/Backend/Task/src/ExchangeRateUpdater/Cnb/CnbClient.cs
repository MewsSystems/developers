using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Cnb;

internal class CnbClient(HttpClient httpClient, ILogger<CnbClient> logger)
{
    private static readonly MediaTypeWithQualityHeaderValue JsonMediaType = new("application/json");
    
    // 💡 if this was _our_ API, I would made this configurable via `HttpClient.BaseUrl` to switch between test and production endpoints,
    //    however since this is completely 3rd party API, I don't expect it to change (and if it does, more things will break anyway)
    private static readonly Uri DailyExchangeRatesUri = new("https://api.cnb.cz/cnbapi/exrates/daily?lang=EN");

    public async Task<CnbExchangeRatesDto> GetCurrentExchangeRates()
    {
        // 💡 since we do HTTP GET, we could omit disposing of the request message since there's no payload, but let's keep analyzer happy
        using var request = new HttpRequestMessage(HttpMethod.Get, DailyExchangeRatesUri);
        request.Headers.Accept.Add(JsonMediaType);

        var response = await httpClient
            .SendAsync(request)
            .ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var rawPayload = await response.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            logger.LogError("Received unexpected status code from CNB API: {StatusCode} {Payload}", response.StatusCode, rawPayload);

            // TODO: return failed result instead of throwing
            throw new InvalidOperationException();
        }

        var payload = await response.Content
            .ReadFromJsonAsync<CnbExchangeRatesDto>()
            .ConfigureAwait(false);

        // 💡 if this was _our_ API, I would trust contract and avoid validation
        if (!IsValid(payload))
        {
            var rawPayload = await response.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            logger.LogError("Received invalid payload from CNB API: {Payload}", rawPayload);

            // TODO: return failed result instead of throwing
            throw new InvalidOperationException();
        }
        
        return payload;
    }

    private static bool IsValid([NotNullWhen(true)] CnbExchangeRatesDto? payload)
    {
        if (payload is null)
        {
            return false;
        }

        var validationContext = new ValidationContext(payload);
        var validationResults = new List<ValidationResult>();

        // we don't really care about the results, we just want to know if the payload is valid overall
        return Validator.TryValidateObject(payload, validationContext, validationResults, validateAllProperties: true);
    }
}

internal class CnbExchangeRatesDto
{
    [JsonPropertyName("rates")]
    [Required]
    public required IReadOnlyCollection<CurrencyRate> Rates { get; init; } = null!;
}

internal class CurrencyRate
{
    [JsonPropertyName("validFor")]
    [Required]
    public required DateTime ValidFor { get; init; }

    [JsonPropertyName("country")]
    [Required]
    public required string CountryName { get; init; } = null!;

    [JsonPropertyName("currency")]
    [Required]
    public required string CurrencyName { get; init; } = null!;
    
    [JsonPropertyName("amount")]
    [Required]
    public required int Amount { get; init; }

    [JsonPropertyName("currencyCode")]
    [Required]
    public required string CurrencyCode { get; init; } = null!;
    
    [JsonPropertyName("rate")]
    [Required]
    public required decimal ExchangeRate { get; init; }
}