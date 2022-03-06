using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Providers.CNB;

public class CNBIcnbExchangeRateService : ICNBExchangeRateService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IXmlExchangeRateParser _rateParser;

    public CNBIcnbExchangeRateService(IHttpClientFactory clientFactory, IXmlExchangeRateParser rateParser, IConfiguration configuration, ILogger<CNBIcnbExchangeRateService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _clientFactory = clientFactory;
        _rateParser = rateParser;
    }

    public async Task<string> GetExchangeRateXmlAsync()
    {
        var httpClient = _clientFactory.CreateClient(nameof(CNBIcnbExchangeRateService));
        var uri = GetUri();
        try
        {

            using var res = await httpClient.GetAsync(uri);
            if (!res.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Request to get the exchange rates from {uri} failed with status code: {(int)res.StatusCode} - {res.StatusCode}.");
            }

            var xmlText = await res.Content?.ReadAsStringAsync();
            return xmlText;
        }
        catch (Exception ex)
        {
            _logger.LogError("$Request to get the exchange rates from {uri} failed", ex);
            throw;
        }
    }

    public async Task<List<ExchangeRate>> GetExchangeRatesAsync()
    {
        var xmlText = await GetExchangeRateXmlAsync();
        return _rateParser.Parse(xmlText);
    }

    private string GetUri()
    {
        const string defaultExchangeRateUri = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml_NOTFOUND";
        var settings = _configuration.GetSection("AppSettings").Get<AppSettings>();
        return settings?.CNBUri ?? defaultExchangeRateUri;
    }
}