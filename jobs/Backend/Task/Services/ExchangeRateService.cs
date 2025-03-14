using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRateService> _logger;   
    public ExchangeRateService(HttpClient httpClient, ILogger<ExchangeRateService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }


    //TODO: Implement the ExchangeRateService class.. return data from given endpoint
    //TODO: Adding caching mechanism to avoid multiple calls to the same endpoint
}
