using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
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
        var request = new HttpRequestMessage(HttpMethod.Get, DailyExchangeRatesUri);
        HttpResponseMessage response = null!;

        try
        {
            response = await httpClient
                .SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var rawPayload = await GetRawPayload(response, cancellationToken)
                    .ConfigureAwait(false);

                logger.UnexpectedStatusCode(response.StatusCode, rawPayload);
                return new CnbUnexpectedStatusError(response.StatusCode);
            }

            var payload = await response.Content
                .ReadFromJsonAsync<CnbExchangeRatesDto>(cancellationToken)
                .ConfigureAwait(false);

            // 💡 if this was _our_ API, I would trust contract and avoid validation
            if (!IsValid(payload))
            {
                var rawPayload = await GetRawPayload(response, cancellationToken)
                    .ConfigureAwait(false);

                logger.InvalidPayload(rawPayload);
                return new CnbInvalidPayloadError(rawPayload);
            }

            return payload;
        }
        catch (TaskCanceledException ex)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw;
            }

            logger.RequestTimedOut(ex);
            return new CnbTimeoutError();
        }
        catch (JsonException ex)
        {
            var rawPayload = await GetRawPayload(response, cancellationToken)
                .ConfigureAwait(false);

            logger.FailedToDeserializePayload(ex, rawPayload);
            return new CnbInvalidPayloadError(ex, rawPayload);
        }
        finally
        {
            request.Dispose();
            response.Dispose();
        }
    }

    private static Task<string> GetRawPayload(HttpResponseMessage response, CancellationToken cancellationToken) =>
        response.Content.ReadAsStringAsync(cancellationToken);

    private static bool IsValid([NotNullWhen(true)] CnbExchangeRatesDto? payload)
    {
        if (payload is null)
        {
            return false;
        }

        return MiniValidator.TryValidate(payload, out _);
    }
}