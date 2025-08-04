using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;

namespace ExchangeRateUpdater.Domain.Repositories;

public class ExchangeRateRepository(IExchangeRateProvider[] exchangeRateProviders) : IExchangeRateRepository
{

    public async Task<Dictionary<string, ExchangeRate[]>> FilterAsync(IEnumerable<Currency> currencies)
    {
        var tasks = exchangeRateProviders.Select(x => new { ExchangeRateProvider = x.Name, Task = x.FetchAllAsync() })
            .ToArray();
        
        try
        {
            await Task.WhenAll(tasks.Select(t => t.Task));
        }
        catch
        {
            // Handle individual task failures
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
        return await FilterAsync([]);
    }
}