using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models.CzechNationalBank;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace ExchangeRateUpdater.Infrastructure;

public class CzechNationalBankExchangeRateClient : IExchangeRateClient<Response>
{
    private readonly IOptions<CzechNationalBankClientOptions> _options;
    private readonly HttpClient _client;
    private readonly ILogger<CzechNationalBankExchangeRateClient> _logger;
    private const string DAILY_EXRATES_PATH = "exrates/daily?lang=EN";

    public CzechNationalBankExchangeRateClient(IOptions<CzechNationalBankClientOptions> options, HttpClient client,
        ILogger<CzechNationalBankExchangeRateClient> logger)
    {
        _options = options;
        _client = client;
        _logger = logger;
    }

    public async Task<Response> RetrieveExchangeRatesAsync()
    {
        try
        {
            var policy = 
                Policy.Handle<TaskCanceledException>()
                .WaitAndRetryAsync(
                    _options.Value.RetryCount,
                    attempt => TimeSpan.FromMilliseconds(_options.Value.TimeoutMs),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(exception, "CzechNationalBankExchangeRateClient_RetryAttempt_{RetryAttempt}",
                            retryCount);
                    });

            var result =
                await policy.ExecuteAsync(async () =>
                    {
                        var result = await _client.GetAsync(DAILY_EXRATES_PATH);
                        return result;
                    }
                );
            
            _logger.LogInformation("CzechNationalBankExchangeRateClient_RetrievedResponse");
            
            if (result.IsSuccessStatusCode)
            {
                var parsedResponse = await result.Content.ReadFromJsonAsync<Response>();
                return parsedResponse;
            }

            var parsedError = await result.Content.ReadFromJsonAsync<ErrorResponse>();
            var errorLog = new ErrorLog(parsedError.description, parsedError.errorCode, result.StatusCode);
            _logger.LogError("CzechNationalBankExchangeRateClient_UnsuccessfulResponse {ErrorLog}", errorLog);
        }
        catch (JsonException ex)
        {
            _logger.LogError("CzechNationalBankExchangeRateClient_DeserializationException {ErrorLog}", ex);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError("CzechNationalBankExchangeRateClient_Timeout {ErrorLog}" ,ex);
        }
        catch (Exception ex)
        {
            _logger.LogError("CzechNationalBankExchangeRateClient_UnexpectedException {ErrorLog}",ex);
        }

        return new EmptyResponse();
    }
}