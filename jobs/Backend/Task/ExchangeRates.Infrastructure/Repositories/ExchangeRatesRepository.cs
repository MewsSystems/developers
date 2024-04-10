using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Repositories;
using ExchangeRates.Http;
using ExchangeRates.Infrastructure.Mappers;
using ExchangeRates.Infrastructure.Models;
using ExchangeRates.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ExchangeRates.Infrastructure.Repositories;

internal class ExchangeRatesRepository : IExchangeRateRepository
{
    private readonly IOptions<CzechNationalBankUrls> _options;

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly IExchangeRateMapper _exchangeRateMapper;

    public ExchangeRatesRepository(IOptions<CzechNationalBankUrls> options, IHttpClientFactory httpClientFactory, IExchangeRateMapper exchangeRateMapper)
    {
        _options = options;
        _httpClientFactory = httpClientFactory;
        _exchangeRateMapper = exchangeRateMapper;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime? day, CancellationToken cancellationToken = default)
    {
        var data = await GetRatesAsync(day, cancellationToken);

        var exRates = JsonSerializer.Deserialize<ExRateDailyResponse>(data);

        return _exchangeRateMapper.Map(exRates);
    }

    private async Task<string> GetRatesAsync(DateTime? day, CancellationToken cancellationToken)
    {
        var requestUriBuilder = new RequestUriBuilder(_options.Value.DailyUrl);
        if (day.HasValue)
            requestUriBuilder.AddQueryParameter(_options.Value.DateQueryParameter, day.Value.ToString("yyyy-MM-dd"));

        var requestUri = requestUriBuilder.Build();

        using var httpClient = _httpClientFactory.CreateClient("PollyClient");
        var response = await httpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
