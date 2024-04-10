using ExchangeRates.Contracts.ExchangeRates;
using ExchangeRates.Http;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure;

internal class ExchangeRatesService : IExchangeRatesService
{
    private readonly IOptions<ExternalApis> _options;

    private readonly IHttpClientFactory _httpClientFactory;

    public ExchangeRatesService(IOptions<ExternalApis> options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IList<ExchangeRate>> GetExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        var data = await GetRatesAsync(cancellationToken);

        var response = JsonSerializer.Deserialize<List<ExchangeRateResponse>>(data);

        return response.Select(x => new ExchangeRate(
            new Currency(x.SourceCode),
            new Currency(x.TargetCode),
            x.Value)).ToList();
    }

    private async Task<string> GetRatesAsync(CancellationToken cancellationToken)
    {
        var requestUri = new RequestUriBuilder(_options.Value.ApiUrl)
            .Build();

        using var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
