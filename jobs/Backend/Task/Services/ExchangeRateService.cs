using ExchangeRateUpdater.DTOs;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRateService> _logger;
    private readonly TimeProvider _timeProvider;

    public ExchangeRateService(HttpClient httpClient, ILogger<ExchangeRateService> logger, TimeProvider timeProvider)
    {
        _httpClient = httpClient;
        _logger = logger;
        _timeProvider = timeProvider;
    }

    public async Task<ExchangeRateListDto> GetExchangeRateListAsync()
    {
        var results = await _httpClient.GetAsync($"cnapi/exrates/daily?date={_timeProvider.GetUtcNow():yyyy-MM-dd}");

        return null;

    }
}
