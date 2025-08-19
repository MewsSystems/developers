using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Errors;
using ExchangeRateUpdater.Services.Handlers;
using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Clients;

public class CnbApiClient : ICnbApiClient
{
    private readonly HttpClient _httpClient;
    private readonly CnbApiConfiguration _configuration;
    private readonly ILogger<CnbApiClient> _logger;

    public CnbApiClient(HttpClient httpClient, ILogger<CnbApiClient> logger, IOptions<CnbApiConfiguration> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<Result<string>> GetExchangeRateDataAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching exchange rates from CNB API");
        return await ExecuteRequestAsync(_configuration.CnbDailyRatesUrl, cancellationToken).ConfigureAwait(false);
    }

    protected virtual async Task<Result<string>> ExecuteRequestAsync(string requestUri, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await _httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("CNB API returned {StatusCode}", response.StatusCode);
                return ErrorHandler.Handle<string>(CnbErrorCode.NetworkError, $"API returned {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Length} characters of exchange rate data", content.Length);
            return Result.Ok(content);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to retrieve data from CNB API");
            return ErrorHandler.Handle<string>(CnbErrorCode.NetworkError, "Network error occurred");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Request to CNB API timed out");
            return ErrorHandler.Handle<string>(CnbErrorCode.TimeoutError, $"Request timed out after {_configuration.TimeoutSeconds} seconds");
        }
    }
}
