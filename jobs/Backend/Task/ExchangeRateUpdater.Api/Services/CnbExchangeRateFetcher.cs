using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Api.Configuration;
using ExchangeRateUpdater.Contract.ExchangeRate;
using ExchangeRateUpdater.Lib.Exception;
using FuncSharp;

namespace ExchangeRateUpdater.Api.Services;

using CnbRates = IEnumerable<CnbExchangeRate>;

public sealed class CnbExchangeRateFetcher(
    ILogger<CnbExchangeRateFetcher> logger,
    HttpClient httpClient,
    ResourcesConfiguration resourcesConfiguration)
    : ICnbExchangeRateFetcher
{
    private readonly string _apiUrl = resourcesConfiguration.CnbApiUrl ??
                                      throw new ArgumentNullException(nameof(resourcesConfiguration.CnbApiUrl));

    public async Task<Try<CnbRates, CnbExchangeRatesFetchError>> FetchExchangeRatesAsync(
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Fetching rates from {_apiUrl}");

        try
        {
            var response = await httpClient.GetStringAsync(_apiUrl, cancellationToken);

            logger.LogDebug($"Received response: {response}");

            var data = JsonSerializer.Deserialize<CnbExchangeRatesResponse>(response);

            if (data is null)
            {
                logger.LogDebug("Unable to deserialize response, data is null");

                return Try.Error<CnbRates, CnbExchangeRatesFetchError>(CnbExchangeRatesFetchError.NoData);
            }

            logger.LogInformation($"Successfully fetched exchange rates. Count {data.Rates.Count()}");

            return Try.Success<CnbRates, CnbExchangeRatesFetchError>(data.Rates);
        }
        catch (TaskCanceledException)
        {
            return Try.Error<CnbRates, CnbExchangeRatesFetchError>(CnbExchangeRatesFetchError.Timeout);
        }
        catch (WebException e) when (e.IsServerError())
        {
            return Try.Error<CnbRates, CnbExchangeRatesFetchError>(CnbExchangeRatesFetchError.ServerException);
        }
        catch (WebException e) when (e.IsConnectionError())
        {
            return Try.Error<CnbRates, CnbExchangeRatesFetchError>(CnbExchangeRatesFetchError.NetworkIssues);
        }
        catch (Exception ex) when (ex is ArgumentNullException or JsonException or NotSupportedException)
        {
            logger.LogError($"Failed to deserialize exchange rates. Error: {ex.Message}");

            return Try.Error<CnbRates, CnbExchangeRatesFetchError>(CnbExchangeRatesFetchError.DataFormat);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error fetching exchange rates: {ex.Message}");

            return Try.Error<CnbRates, CnbExchangeRatesFetchError>(CnbExchangeRatesFetchError.Unknown);
        }
    }
}