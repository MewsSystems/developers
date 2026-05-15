using CNB.Client.Exceptions;
using CNB.Client.Models;
using ExchangeRates.Domain;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CNB.Client
{
    /// <summary>
    /// Integration client for CNB bank.
    /// </summary>
    public class CnbClient : IBankClient
    {
        private readonly HttpClient _client;

        public CnbClient(HttpClient client)
        {
            _client = client;
        }

        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            WriteIndented = true
        };

        public async Task<List<ExchangeRate>> GetRatesDaily(
             DateOnly date,
             CancellationToken cancellationToken = default)
        {
            var formatedDate = date.ToString("yyyy-MM-dd");
            using var request = new HttpRequestMessage(HttpMethod.Get, $"cnbapi/exrates/daily?date={formatedDate}&lang=EN");
            using var response = await _client.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            try
            {
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<CbnResponse>(_options, cancellationToken);
                return result?.Rates.Select(x => x.ToDomain()).ToList() ??
                          throw new JsonDeserializationException($"Cannot deserialize json for the exchange rate");
            }
            catch (JsonException ex)
            {
                throw new JsonDeserializationException($"Cannot deserialize json for the exchange rate", ex);
            }
            catch (HttpRequestException ex)
            {
                throw new BankApiException("CNB couldn't handle the request for exchange rates", ex);
            }
        }
    }

    public class CbnResponse
    {
        public required List<CnbRate> Rates { get; set; }
    }
}