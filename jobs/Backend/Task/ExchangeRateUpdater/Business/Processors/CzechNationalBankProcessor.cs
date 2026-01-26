using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Mapping;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Models.CzechNationalBank;
using ExchangeRateUpdater.Models.CzechNationalBankResponse;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Business.Processors;

public class CzechNationalBankProcessor : IExchangeRateProcessor 
{
    private readonly IExchangeRateClient<Response> _client;
    private readonly IExchangeRateMapper<RecordedRate> _mapper;
    private readonly ILogger<CzechNationalBankProcessor> _logger;

    public CzechNationalBankProcessor(
        IExchangeRateClient<Response> client,
        IExchangeRateMapper<RecordedRate> mapper,
        ILogger<CzechNationalBankProcessor> logger)
    {
        _client = client;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<Result<IEnumerable<ExchangeRate>>> ProcessExchangeRates(string[] currenciesToProcess)
    { 
        _logger.LogInformation("CzechNationalBankProcessor_ProcessingCurrencies");
        
        var processedRates = new List<ExchangeRate>();
       var responseToProcess = await _client.RetrieveExchangeRatesAsync();

       if (responseToProcess is EmptyResponse)
       {
           return Result<IEnumerable<ExchangeRate>>.Fail("CzechNationalBankProcessor_UnexpectedClientError");
       }

       var dictionaryOfReturnedRates =
           responseToProcess.Rates.ToDictionary(key => key.CurrencyCode, StringComparer.OrdinalIgnoreCase);

       foreach (var desiredCurrency in currenciesToProcess)
       {
           var haveFoundEntry = dictionaryOfReturnedRates.TryGetValue(desiredCurrency, out var value);
           if (!haveFoundEntry)
           {
               _logger.LogWarning("CzechNationalBankProcessor_CurrencyNotFound_{DesiredCurrency}", desiredCurrency);
               continue;
           }
           
           var mappedExchangeRate = _mapper.MapToExchangeRate(value);
           processedRates.Add(mappedExchangeRate);
       }
       
       _logger.LogInformation("CzechNationalBankProcessor_FinishedProcessingCurrencies");
       return Result<IEnumerable<ExchangeRate>>.Ok(processedRates);
    }
}