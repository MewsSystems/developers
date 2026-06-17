using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExchangeRateProvider.Exceptions;
using ExchangeRateProvider.Implementations.CzechNationalBank.Models;

namespace ExchangeRateProvider.Implementations.CzechNationalBank;

internal class CzechNationalBankApi : ICzechNationalBankApi
{
    private readonly IHttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOpts = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public CzechNationalBankApi(IHttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.cnb.cz");
        _httpClient.Timeout = TimeSpan.FromSeconds(8);
        _httpClient.MaxRetries = 3;
    }

    public async Task<ExRateDailyResponse> GetExratesDaily(DateTimeOffset date)
    {
        var dateParam = date.UtcDateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var queryParams = $"date={dateParam}&lang=EN";
        var url = $"cnbapi/exrates/daily?{queryParams}";

        var httpResponse = await _httpClient.Get(url);

        return await ProcessHttpResponse<ExRateDailyResponse>(httpResponse);
    }

    internal async Task<T> ProcessHttpResponse<T>(HttpResponseMessage httpResponse)
    {
        var httpBodyResponseStr = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            var exception = new UnexpectedException("http response not successful");
            exception.Data.Add("Url", httpResponse.RequestMessage?.RequestUri);
            exception.Data.Add("StatusCode", httpResponse.StatusCode);
            exception.Data.Add("ResponseBody", httpBodyResponseStr);
            throw exception;
        }

        if (string.IsNullOrWhiteSpace(httpBodyResponseStr))
            throw new UnexpectedException("http response body is null or empty");

        T deserializedResponse;
        try
        {
            deserializedResponse = JsonSerializer.Deserialize<T>(httpBodyResponseStr, _jsonSerializerOpts)!;
        }
        catch (Exception)
        {
            var exception = new UnexpectedException("http response body failed to deserialize");
            exception.Data.Add("ResponseBody", httpBodyResponseStr);
            throw exception;
        }
        return deserializedResponse;
    }
}