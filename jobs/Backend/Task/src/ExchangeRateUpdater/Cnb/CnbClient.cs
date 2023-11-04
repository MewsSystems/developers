using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using W4k.Either;

namespace ExchangeRateUpdater.Cnb;

internal class CnbClient(HttpClient httpClient, ILogger<CnbClient> logger) : ICnbClient
{
    private static readonly MediaTypeWithQualityHeaderValue JsonMediaType = new("application/json");

    // 💡 if this was _our_ API, I would made this configurable via `HttpClient.BaseUrl` to switch between test and production endpoints,
    //    however since this is completely 3rd party API, I don't expect it to change (and if it does, more things will break anyway)
    private static readonly Uri DailyExchangeRatesUri = new("https://api.cnb.cz/cnbapi/exrates/daily?lang=EN");

    public async Task<Either<CnbExchangeRatesDto, CnbError>> GetCurrentExchangeRates(CancellationToken cancellationToken)
    {
        // 💡 since we do HTTP GET, we could omit disposing of the request message since there's no payload, but let's keep analyzer happy
        using var request = new HttpRequestMessage(HttpMethod.Get, DailyExchangeRatesUri);
        request.Headers.Accept.Add(JsonMediaType);

        var response = await httpClient
            .SendAsync(request, cancellationToken)
            .ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var rawPayload = await response.Content
                .ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false);

            logger.UnexpectedStatusCode(response.StatusCode, rawPayload);
            return new CnbError();
        }

        var payload = await response.Content
            .ReadFromJsonAsync<CnbExchangeRatesDto>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        // 💡 if this was _our_ API, I would trust contract and avoid validation
        if (!IsValid(payload))
        {
            var rawPayload = await response.Content
                .ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false);

            logger.InvalidPayload(rawPayload);
            return new CnbError();
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