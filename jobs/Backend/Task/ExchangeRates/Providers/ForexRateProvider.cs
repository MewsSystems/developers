using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExchangeRatesService.Models;
using ExchangeRatesService.Providers.Interfaces;

namespace ExchangeRatesService.Providers;

public class ForexRateProvider(HttpClient httpClient, TimeProvider timeProvider): IRatesProvider,  IAsyncDisposable
{
    public ValueTask DisposeAsync()
    {
        Console.WriteLine($"{nameof(ForexRateProvider)}.Dispose()");
        return ValueTask.CompletedTask;
    }

    public async IAsyncEnumerable<ExchangeRate> GetRatesAsync(IEnumerable<Currency> currencies,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var rates = await GetExchangeRates(currencies);
            
        foreach (var rate in rates)
        {
            await Task.Delay(500);
            yield return rate;
        }
            
        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates.");
    }

    public IAsyncEnumerable<ExchangeRate> GetRatesReverseAsync(IEnumerable<Currency> currencies, decimal amount,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            WriteIndented = true,
            Converters =
            {
                new CurrencyJsonConverter()
            }
        };
        
        var lastMonth = timeProvider.GetUtcNow().AddMonths(-1).Month;
        
        var response  = (await httpClient.GetFromJsonAsync<ExchangeRateIterator>(
            $"/cnbapi/fxrates/daily-month?lang=EN&yearMonth={lastMonth:yyyy-MM}",
            options,
            cancellationToken: cancellationToken));
        
        if (response.rates is null)
        {
            throw new NullReferenceException("CNB API response does not contain a valid list of exchange rate fixings");
        }
        
        return response.rates.Where(rate => currencies.Select(x => x.Code).Contains(rate.SourceCurrency.Code));
        
    }
}