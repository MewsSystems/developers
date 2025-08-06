using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.Repositories;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly Dictionary<string, IExchangeRateProvider> _providers;
    private readonly ILogger<ExchangeRateRepository> _logger;

    public ExchangeRateRepository(IEnumerable<IExchangeRateProvider> exchangeRateProviders, ILogger<ExchangeRateRepository> logger)
    {
        _providers = exchangeRateProviders.ToDictionary(p => p.Name, p => p);
        _logger = logger;
    }

    public async Task<Dictionary<string, ExchangeRate[]>> FilterAsync(IEnumerable<Currency> currencies)
    {
        var currencyArray = currencies as Currency[] ?? currencies.ToArray();
        _logger.LogInformation("Filtering exchange rates for {CurrencyCount} currencies from {ProviderCount} providers", currencyArray.Length, _providers.Count);
        
        try
        {
            var tasks = _providers.Select(x => new { ExchangeRateProvider = x.Key, Task = x.Value.FetchAllCurrentAsync() })
                .ToArray();
            
            _logger.LogDebug("Started fetching rates from {ProviderCount} providers", tasks.Length);
            
            try
            {
                await Task.WhenAll(tasks.Select(t => t.Task));
                _logger.LogDebug("All provider tasks completed");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Some provider tasks failed, continuing with successful ones");
            }

            var currenciesSet = new HashSet<Currency>(currencyArray);
            var successfulTasks = tasks.Where(t => t.Task.Status == TaskStatus.RanToCompletion).ToArray();
            
            _logger.LogInformation("Successfully retrieved rates from {SuccessfulCount}/{TotalCount} providers", successfulTasks.Length, tasks.Length);
            
            // Group successful results
            var result = successfulTasks
                .GroupBy(t => t.ExchangeRateProvider)
                .ToDictionary(
                    g => g.Key,
                    g => g.SelectMany(t => t.Task.Result).Where(x => currenciesSet.Count == 0 || currenciesSet.Contains(x.SourceCurrency)).ToArray()
                );
            
            var totalRates = result.Values.Sum(rates => rates.Length);
            _logger.LogInformation("Filtered {TotalRates} exchange rates from {ProviderCount} providers", totalRates, result.Count);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to filter exchange rates for currencies: {Currencies}", string.Join(", ", currencyArray.Select(c => c.Code)));
            throw;
        }
    }

    public async Task<Dictionary<string, ExchangeRate[]>> GetAllAsync()
    {
        _logger.LogInformation("Getting all exchange rates from {ProviderCount} providers", _providers.Count);
        try
        {
            var result = await FilterAsync([]);
            _logger.LogInformation("Successfully retrieved all exchange rates from {ProviderCount} providers", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get all exchange rates, returning empty result");
            return new Dictionary<string, ExchangeRate[]>();
        }
    }

    public async Task<Dictionary<string, ExchangeRate[]>> GetFromProviderAsync(string providerName, IEnumerable<Currency> currencies)
    {
        var currencyFilter = currencies as Currency[] ?? currencies.ToArray();
        _logger.LogInformation("Getting exchange rates from provider '{ProviderName}' for {CurrencyCount} currencies", providerName, currencyFilter.Length);
        
        try
        {
            if (!_providers.TryGetValue(providerName, out var provider))
            {
                var availableProviders = string.Join(", ", _providers.Keys);
                _logger.LogError("Provider '{ProviderName}' not found. Available providers: {AvailableProviders}", providerName, availableProviders);
                throw new ArgumentException($"Provider '{providerName}' not found. Available providers: {availableProviders}");
            }

            _logger.LogDebug("Fetching rates from provider '{ProviderName}'", providerName);
            var rates = await provider.FetchAllCurrentAsync();
            _logger.LogInformation("Retrieved {RateCount} rates from provider '{ProviderName}'", rates.Length, providerName);
            
            var currenciesSet = new HashSet<Currency>(currencyFilter);
            var filteredRates = rates.Where(x => currenciesSet.Contains(x.SourceCurrency)).ToArray();
            
            _logger.LogInformation("Filtered {FilteredCount}/{TotalCount} rates from provider '{ProviderName}' for requested currencies", 
                filteredRates.Length, rates.Length, providerName);
            
            return new Dictionary<string, ExchangeRate[]>
            {
                [providerName] = filteredRates
            };
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get exchange rates from provider '{ProviderName}' for currencies: {Currencies}, returning empty result", 
                providerName, string.Join(", ", currencyFilter.Select(c => c.Code)));
            return new Dictionary<string, ExchangeRate[]>();
        }
    }
} 