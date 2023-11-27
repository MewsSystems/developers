using Adapter.Http.CNB.Dtos.Response;
using Adapter.Http.CNB.Mappers;
using Adapter.Http.CNB.Resiliency;
using Domain.Entities;
using Domain.Ports;
using Flurl;
using Newtonsoft.Json;
using Serilog.Core;

namespace Adapter.Http.CNB.Repositories;

public class ExchangeRatesRepository : IExchangeRatesRepository
{
    private readonly Logger _logger;
    private readonly CNBSettings _settings;
    
    public ExchangeRatesRepository(CNBSettings settings, Logger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<List<ExchangeRate>> GetDailyExchangeRatesAsync(DateTime date, CancellationToken cancellationToken)
    {
        ExchangeRatesResponseDto? result;
        
        using (var httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(_settings.BaseAddress);
            
            var url = "/cnbapi/exrates/daily"
                .SetQueryParams(new
                {
                    date = date.ToString("yyyy-MM-dd"),
                    lang = "EN"
                });

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = url.ToUri()
            };

            var response = await httpClient.SendWithRetryAsync(async () =>
            {
                return await httpClient.SendAsync(httpRequestMessage, cancellationToken);
            });

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = response.Content
                    .ReadAsStringAsync(cancellationToken)
                    .GetAwaiter().GetResult();

                result = JsonConvert.DeserializeObject<ExchangeRatesResponseDto>(jsonResult);
            }
            else
            {
                throw new HttpRequestException($"Failed to request ExchangeRates. Got {response.StatusCode} back.");
            }
        }

        return result.Rates.Select(rate => rate.ToExchangeRate()).ToList();
    }
}