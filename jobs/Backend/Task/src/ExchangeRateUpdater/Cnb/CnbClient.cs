using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MiniValidation;
using W4k.Either;

namespace ExchangeRateUpdater.Cnb;

internal class CnbClient(HttpClient httpClient, ILogger<CnbClient> logger) : ICnbClient
{
    // 💡 if this was _our_ API, I would made this configurable via `HttpClient.BaseUrl` to switch between test and production endpoints,
    //    however since this is completely out of our hands, I hardcoded it (change will require recompilation/redeploy anyway)
    private static readonly Uri DailyExchangeRatesUri = new("https://api.cnb.cz/cnbapi/exrates/daily?lang=EN");

    public async Task<Either<CnbExchangeRatesDto, CnbError>> GetCurrentExchangeRates(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, DailyExchangeRatesUri);
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

        return MiniValidator.TryValidate(payload, out _);
    }
}