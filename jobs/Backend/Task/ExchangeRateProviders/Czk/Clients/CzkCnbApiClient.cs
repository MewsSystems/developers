using ExchangeRateProviders.Czk.Model;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Polly;
using ExchangeRateProviders.Czk.Config;

namespace ExchangeRateProviders.Czk.Clients;

public class CzkCnbApiClient : ICzkCnbApiClient
{
	private readonly HttpClient _httpClient;
	private readonly ILogger<CzkCnbApiClient> _logger;
	private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;
	private static readonly Uri Endpoint = new(Constants.CnbApiDailyRatesEndpoint);
	private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

	private static IAsyncPolicy<HttpResponseMessage> DefaultPolicy =>
		Policy<HttpResponseMessage>
			.Handle<HttpRequestException>()
			.OrResult(r => (int)r.StatusCode >= 500 || (int)r.StatusCode == 429)
			.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2));

	public CzkCnbApiClient(
		HttpClient httpClient,
		ILogger<CzkCnbApiClient> logger,
		IAsyncPolicy<HttpResponseMessage>? retryPolicy = null)
	{
		_httpClient = httpClient;
		_logger = logger;
		_retryPolicy = retryPolicy ?? DefaultPolicy;
	}

	public async Task<IReadOnlyList<CnbApiExchangeRateDto>> GetDailyRatesRawAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Requesting CNB rates from {Endpoint}", Endpoint);
		var response = await _retryPolicy.ExecuteAsync(
			() => _httpClient.GetAsync(Endpoint, HttpCompletionOption.ResponseHeadersRead, cancellationToken));

		response.EnsureSuccessStatusCode();

		await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
		var root = await JsonSerializer.DeserializeAsync<CnbApiCzkExchangeRateResponse>(stream, JsonOptions, cancellationToken).ConfigureAwait(false);
		var list = root?.Rates ?? new List<CnbApiExchangeRateDto>();

		if (list.Count == 0)
			_logger.LogWarning("CNB rates response empty.");
		else
			_logger.LogInformation("CNB returned {Count} raw rates.", list.Count);

		return list;
	}
}
