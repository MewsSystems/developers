namespace CNB.ApiClient;

public class CNBApiClient(HttpClient client) : ICNBApiClient
{
    private readonly HttpClient _client = client;

    private readonly JsonSerializerOptions _options =
        new()
        {
            PropertyNameCaseInsensitive = true,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            WriteIndented = true
        };

    public async Task<ExratesDailyResponse> GetDailyExrates(
        DateOnly date,
        CancellationToken cancellationToken
    )
    {
        var formattedDate = date.ToString("yyyy-MM-dd");
        var url = $"cnbapi/exrates/daily?date={formattedDate}&lang=EN";

        try
        {
            var response = await _client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, url),
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken
            );

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ExratesDailyResponse>(
                _options,
                cancellationToken
            );
            return result;
        }
        catch (HttpRequestException ex)
        {
            throw new CNBApiException("CNB Api failed to handle exrates/daily request.", ex);
        }
    }
}
