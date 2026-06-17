using Mews.ERP.AppService.Features.Fetch.Models;
using Mews.ERP.AppService.Features.Fetch.Networking.Providers.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Repositories.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Mews.ERP.AppService.Features.Fetch.Services;

public class FetchService : IFetchService
{
    private readonly ICurrenciesRepository currenciesRepository;
    
    private readonly ICnbExchangeRatesProvider ratesProvider;
    
    private readonly ILogger<FetchService> logger;
    
    public FetchService(
        ICurrenciesRepository currenciesRepository, 
        ICnbExchangeRatesProvider ratesProvider, 
        ILogger<FetchService> logger)
    {
        this.currenciesRepository = currenciesRepository;
        this.ratesProvider = ratesProvider;
        this.logger = logger;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
    {
        logger.LogInformation($"Starting to fetch currencies and exchange rates...");
        
        var storedCurrencies = await currenciesRepository.GetAllAsync();

        logger.LogDebug($"Retrieved currencies from database: {JsonConvert.SerializeObject(storedCurrencies, Formatting.Indented)}");
        
        var rates = await ratesProvider.GetExchangeRatesAsync(storedCurrencies);

        logger.LogInformation($"Fetched exchange rates: {JsonConvert.SerializeObject(rates, Formatting.Indented)}");
        
        return rates;
    }
}