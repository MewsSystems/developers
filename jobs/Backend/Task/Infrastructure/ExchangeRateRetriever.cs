using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure;

public class ExchangeRateRetriever : IExchangeRateRetriever
{
    private readonly HttpClient _httpClient;
    private readonly string _url;
    private readonly IExchangeRateMapper _mapper;

    public ExchangeRateRetriever(HttpClient httpClient, IConfiguration configuration, IExchangeRateMapper mapper)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _url = configuration["BankSettings:Url"];
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<Result<ExchangeRate[]>> GetExchangeRatesAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(_url);

            return _mapper.Map(response.AsSpan());
        }
        catch (Exception ex)
        {
            return Result<ExchangeRate[]>.Failure($"Error getting exchange rates: {ex.Message}");
        }
    }
}
