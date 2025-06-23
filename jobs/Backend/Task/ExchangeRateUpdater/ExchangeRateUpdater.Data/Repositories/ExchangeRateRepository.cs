using ExchangeRateUpdater.Data.Interfaces;
using ExchangeRateUpdater.Data.Responses;
using ExchangeRateUpdater.Models.Models;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Data.Repositories;
public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly IConfiguration _configuration;
    private readonly IExchangeRateCacheRepository _cacheRepository;  // Instancia de IMemoryCache
    private readonly ILogger<ExchangeRateRepository> _logger;

    public ExchangeRateRepository(IConfiguration configuration, IExchangeRateCacheRepository cacheRepository, ILogger<ExchangeRateRepository> logger)
    {
        _configuration = configuration;
        _cacheRepository = cacheRepository;
        _logger = logger;
    }
    public async Task<List<ExchangeRate>> GetExchangeRatesByDateAsync(DateTime date, CancellationToken cancellationToken)
    {
        ExchangeRatesResponseDto? exchangeRates;

        // Try get exchange rates from caché first
        exchangeRates = _cacheRepository.GetExchangeRates(date);

        //If no results from cache, get the data from the client
        if (!exchangeRates.Rates.Any())
        {
            exchangeRates = await GetExchangeRates(date, cancellationToken);

            //Set the data in cache
            _cacheRepository.SetExchangeRates(exchangeRates);
        }

        var result = exchangeRates.Rates.Select(rate => MapToExchangeRate(rate)).ToList();        

        return result;
    }

    private async Task<ExchangeRatesResponseDto> GetExchangeRates(DateTime date, CancellationToken cancellationToken)
    {
        var resul = new ExchangeRatesResponseDto();

        try
        {
            using (var httpClient = new HttpClient())
            {
                if (string.IsNullOrEmpty(_configuration["ExchangeRateUrl"])) 
                {
                    _logger.LogError("Czech National Bank Url not defined.");
                    throw new ApplicationException("Czech National Bank Url not defined.");
                }

                string baseUrl = _configuration["ExchangeRateUrl"] ;

                resul = await baseUrl
                    .SetQueryParams(new
                    {
                        date = date.ToString("yyyy-MM-dd"),
                        lang = "EN"
                    })
                    .GetJsonAsync<ExchangeRatesResponseDto>();
            }
        }
        catch (FlurlHttpException ex)
        {
            _logger.LogError(ex.Message);
            throw new ApplicationException(ex.Message);
        }

        _logger.LogInformation($"{resul.Rates.Count} exchange rates have been obtained.");
        return resul;
    }

    private ExchangeRate MapToExchangeRate(ExchangeRateDto exchangeRateDto)
    {
        return new ExchangeRate(
            new Currency("CZK"), //CNB only retrieve rates for CZK
            new Currency(exchangeRateDto.CurrencyCode),
            exchangeRateDto.Rate,
            exchangeRateDto.ValidFor);
    }
}
