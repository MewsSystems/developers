using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Infrastucture;

public class ApiClient
{
    private static readonly MemoryCache Cache = new(new MemoryCacheOptions()); // TODO Tweak options for needs or DI
    private readonly Currency _countryCurrency;
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _countryCurrency = new Currency("CZK"); // TODO Encapsulate into config?
    }

    public async IAsyncEnumerable<ExchangeRate> GetAllExchangeRates(DateOnly? date = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        date ??= DateOnly.FromDateTime(DateTime.Today);

        if (!Cache.TryGetValue(date, out CzRatesResponse? response))
        {
            response = await _httpClient.GetFromJsonAsync<CzRatesResponse>(
                           $"exrates/daily?date={date:yyyy-MM-dd}&lang=EN", cancellationToken);
            Cache.Set(date, response);
        }

        if (response != null)
            foreach (var rate in response.Rates)
                yield return new ExchangeRate(new Currency(rate.CurrencyCode), _countryCurrency,
                    rate.Rate / rate.Amount);
    }
}