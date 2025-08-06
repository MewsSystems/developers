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
        var tasks = _providers.Select(x => new { ExchangeRateProvider = x.Key, Task = x.Value.FetchAllCurrentAsync() })
            .ToArray();
        
        try
        {
            await Task.WhenAll(tasks.Select(t => t.Task));
        }
        catch
        {
            // ignored
        }

        var currenciesSet = new HashSet<Currency>(currencies);
        
        // Group successful results
        return tasks
            .Where(t => t.Task.Status == TaskStatus.RanToCompletion)
            .GroupBy(t => t.ExchangeRateProvider)
            .ToDictionary(
                g => g.Key,
                g => g.SelectMany(t => t.Task.Result).Where(x => currenciesSet.Contains(x.SourceCurrency)).ToArray()
            );
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
            _logger.LogError(ex, "Failed to get all exchange rates");
            throw;
        }
    }

    public async Task<Dictionary<string, ExchangeRate[]>> GetFromProviderAsync(string providerName, IEnumerable<Currency> currencies)
    {
        _logger.LogInformation("Getting exchange rates from provider '{ProviderName}' for {CurrencyCount} currencies", providerName, currencies.Count());
        
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
            
            var currenciesSet = new HashSet<Currency>(currencies);
            var filteredRates = rates.Where(x => currenciesSet.Contains(x.SourceCurrency)).ToArray();
            
            _logger.LogInformation("Filtered {FilteredCount}/{TotalCount} rates from provider '{ProviderName}' for requested currencies", 
                filteredRates.Length, rates.Length, providerName);
            
            return new Dictionary<string, ExchangeRate[]>
            {
                [providerName] = filteredRates
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get exchange rates from provider '{ProviderName}' for currencies: {Currencies}", 
                providerName, string.Join(", ", currencies.Select(c => c.Code)));
            throw;
        }
    }
} 