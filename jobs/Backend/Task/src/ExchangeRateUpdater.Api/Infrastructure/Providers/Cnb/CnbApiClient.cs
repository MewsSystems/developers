using System.Text.Json;
using ExchangeRateUpdater.Api.Domain.Common;
using ExchangeRateUpdater.Api.Infrastructure.Interfaces;
using ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb.Models;

namespace ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb;

public sealed class CnbApiClient(HttpClient httpClient, ILogger<CnbApiClient> logger) : IExternalExchangeRateClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<Result<CnbExRateDailyResponse>> GetDailyRatesAsync(
        DateOnly? date = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = BuildUrl(date);
            logger.LogInformation("Fetching daily exchange rates from CNB API: {Url}", url);

            var response = await httpClient.GetFromJsonAsync<CnbExRateDailyResponse>(
                url, JsonOptions, cancellationToken);

            if (response is null or { Rates.Length: 0 })
            {
                logger.LogWarning("CNB API returned empty rates for date {Date}", date);
                return Result<CnbExRateDailyResponse>.Failure(ExchangeRateErrors.NoRatesAvailable);
            }

            logger.LogInformation("Successfully fetched {Count} raw rates from CNB API", response.Rates.Length);
            return Result<CnbExRateDailyResponse>.Success(response);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error while fetching exchange rates from CNB API");
            return Result<CnbExRateDailyResponse>.Failure(ExchangeRateErrors.SourceUnavailable);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            logger.LogError(ex, "Timeout while fetching exchange rates from CNB API");
            return Result<CnbExRateDailyResponse>.Failure(ExchangeRateErrors.SourceUnavailable);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to deserialize CNB API response");
            return Result<CnbExRateDailyResponse>.Failure(ExchangeRateErrors.SourceUnavailable);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong during CNB API call");
            return Result<CnbExRateDailyResponse>.Failure(ExchangeRateErrors.SourceUnavailable);
        }
    }

    private static string BuildUrl(DateOnly? date)
    {
        var url = "/cnbapi/exrates/daily?lang=EN";
        if (date.HasValue)
            url += $"&date={date.Value:yyyy-MM-dd}";
        return url;
    }
}
