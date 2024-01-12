using System.Globalization;
using System.Text.Json;
using ExchangeRateProvider.Exceptions;
using ExchangeRateProvider.Implementations.CzechNationalBank.Models;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Implementations.CzechNationalBank;

class ExchangeRateProviderCzechNationalBank : IExchangeRateProvider, IDisposable
{
    private const int MAX_RETRIES = 3;
    private readonly HttpClient _httpClient;

    public ExchangeRateProviderCzechNationalBank()
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.cnb.cz"),
            Timeout = TimeSpan.FromSeconds(8)
        };
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    public GetExchangeRates GetExchangeRates => GetExchangeRatesImpl;

    public async Task<ICollection<ExchangeRate>> GetExchangeRatesImpl(IEnumerable<Currency> currencies, DateTimeOffset date)
    {
        var dateParam = date.UtcDateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var queryParams = $"date={dateParam}&lang=EN";

        var httpResponse = await HandleHttpRetries(() => _httpClient.GetAsync($"cnbapi/exrates/daily?{queryParams}"));

        var deserializedResponse = await ProcessHttpResponse<ExRateDailyResponse>(httpResponse);

        return deserializedResponse
            .Rates
            .Where(res => currencies.Any(curr => curr.Code.Equals(res.CurrencyCode, StringComparison.OrdinalIgnoreCase)))
            .Select(res =>
                new ExchangeRate(
                    new Currency("CZK"),
                    new Currency(res.CurrencyCode.ToUpperInvariant()),
                    res.Rate
                )
            ).ToArray();
    }

    private async Task<HttpResponseMessage> HandleHttpRetries(Func<Task<HttpResponseMessage>> httpClientFn)
    {
        for (int i = 0; i < MAX_RETRIES; i++)
        {
            try
            {
                return await httpClientFn() ?? throw new UnexpectedException("http response is null");
            }
            catch (Exception)
            {
                if (i == MAX_RETRIES - 1)
                    throw;
            }
        }
        throw new Exception();
    }

    private async Task<T> ProcessHttpResponse<T>(HttpResponseMessage httpResponse)
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
            deserializedResponse = JsonSerializer.Deserialize<T>(httpBodyResponseStr)!;
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