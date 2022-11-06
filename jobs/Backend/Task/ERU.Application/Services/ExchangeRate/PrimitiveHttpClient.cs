using ERU.Application.Interfaces;

namespace ERU.Application.Services.ExchangeRate;

public class PrimitiveHttpClient : IHttpClient
{
	private readonly HttpClient _client = new();

	public async Task<string> GetStringAsync(string uri, CancellationToken token)
	{
		return await _client.GetStringAsync(uri, token);
	}
}