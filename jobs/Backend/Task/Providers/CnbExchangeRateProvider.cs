using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ExchangeRateUpdater.CsvParser;
using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Utils;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Providers;

public class CnbExchangeRateProvider : IExchangeRateProvider
{
    private readonly ILogger<CnbExchangeRateProvider> _logger;
    private List<IExchangeRateEndpoint> _endpoints;
    private readonly IHttpHandler _httpHandler;

    private static readonly Currency BankNationalCurrency = new Currency("CZK");
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICnbCsvReader _cnbCsvReader;
    private readonly IDictionary<Currency, ExchangeRate> _exchangeRates = new Dictionary<Currency, ExchangeRate>();
         
    public CnbExchangeRateProvider(ILogger<CnbExchangeRateProvider> logger, 
        IHttpHandler httpHandler, 
        IDateTimeProvider dateTimeProvider,
        ICnbCsvReader cnbCsvReader)
    {
        _logger = logger.NotNull();
        _httpHandler = httpHandler.NotNull();
        _dateTimeProvider = dateTimeProvider.NotNull();
        _cnbCsvReader = cnbCsvReader.NotNull();
    }

    public void SetEndpoints(IEnumerable<IExchangeRateEndpoint> endpoints)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.LogTrace("Setting a new endpoints");
        _endpoints = endpoints.NotNull().ToList();
    }

    public void FetchRates()
    {
        foreach (var endpoint in _endpoints)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Trace))
                    _logger.LogTrace($"Fetching rates from endpoint {endpoint.Name}");

                var dataAcquired = GetDataFromEndpoint(endpoint, out var httpResponse);
                if (!dataAcquired)
                    continue;

                var records = ParseEndpointResponse(httpResponse);

                AddOrUpdateExchangeRates(records);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to obtain rates from endpoint {endpoint.Name}: {e}");
            }
        }
    }

    private bool GetDataFromEndpoint(IExchangeRateEndpoint endpoint, out HttpResponseMessage httpResponse)
    {
        var url = UrlFormatter.Construct(_dateTimeProvider.UtcNow(), endpoint.Url, endpoint.Parameters);
        httpResponse = _httpHandler.Get(url);
        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogWarning(
                $"Failed to obtain rates from endpoint {endpoint.Name}: HTTP response code {httpResponse.StatusCode}");
            return false;
        }

        return true;
    }

    private IEnumerable<CnbExchangeRateModel> ParseEndpointResponse(HttpResponseMessage httpResponse)
    {
        var responseStream = httpResponse.Content.ReadAsStream();
        return _cnbCsvReader.GetRecords(responseStream);
    }

    private void AddOrUpdateExchangeRates(IEnumerable<CnbExchangeRateModel> records)
    {
        foreach (var record in records)
        {
            var newRate = new ExchangeRate(record.Code, BankNationalCurrency, record.Rate / record.Amount);

            if (_exchangeRates.ContainsKey(record.Code))
            {
                _exchangeRates[record.Code] = newRate;
            }
            else
            {
                _exchangeRates.Add(record.Code, newRate);
            }
        }
    }

    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var ret = new List<ExchangeRate>();
        
        foreach (var requestedCurrency in currencies)
        {
            if (_exchangeRates.TryGetValue(requestedCurrency, out var exchangeRate))
            {
                ret.Add(exchangeRate);
            }
        }

        return ret;
    }
}
