using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExchangeRatesService.Models;
using ExchangeRatesService.Providers.Interfaces;

namespace ExchangeRatesService.Providers;

public class ExchangeRateProvider(HttpClient httpClient): IRatesProvider, IAsyncDisposable
    {   
        /*private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ExchangeRateProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }*/

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

        public async IAsyncEnumerable<ExchangeRate> GetRatesReverseAsync(IEnumerable<Currency> currencies, decimal amount, 
            CancellationToken token = default)
        {
            var rates = await GetExchangeRates(currencies);

            foreach (var rate in rates)
            {
                await Task.Delay(500);
                var d = rate.Amount != 100 ? rate.Value : rate.Value  * rate.Amount;
                var c = amount * d;
                var reverseRate = rate.Amount != 100 ?  amount / c :  (amount / c) * rate.Amount;
                yield return new ExchangeRate(rate.TargetCurrency, rate.SourceCurrency, reverseRate, rate.Amount);
            }
            
            Console.WriteLine($"Successfully converted {rates.Count()} exchange rates.");
        }
        
        private async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
                WriteIndented = true,
                Converters =
                {
                    new CurrencyJsonConverter()
                },
                //DefaultBufferSize = 512
            };

            /*var responseMessage =  await httpClient.GetAsync("/cnbapi/exrates/daily?lang=EN");
            var content = responseMessage.Content.ReadAsStringAsync().Result;
            var rates = JsonSerializer.Deserialize<ExchangeRateIterator>(content, options)!.rates;*/
            
            var response = (await httpClient.GetFromJsonAsync<ExchangeRateIterator>("/cnbapi/exrates/daily?lang=EN",options,
                cancellationToken: cancellationToken));
           
            if (response.rates is null)
            {
                throw new NullReferenceException("CNB API response does not contain a valid list of exchange rate fixings");
            }
        
            return response.rates.Where(rate => currencies.Select(x => x.Code).Contains(rate.SourceCurrency.Code));
        }

        
        public ValueTask DisposeAsync()
        {
            Console.WriteLine($"{nameof(ExchangeRateProvider)}.Dispose()");
            return ValueTask.CompletedTask;
        }
    }