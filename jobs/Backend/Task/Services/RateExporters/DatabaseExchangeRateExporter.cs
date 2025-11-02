using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services.RateExporters;

/// <summary>
///     Exports exchange rate information to a database using a repository pattern.
///     Implements <see cref="IExchangeRateExporter" /> to persist exchange rates as <see cref="ExchangeRateEntity" />
///     records.
/// </summary>
public class DatabaseExchangeRateExporter : IExchangeRateExporter
{
    private readonly ILogger<DatabaseExchangeRateExporter> _logger;
    private readonly IRepository<ExchangeRateEntity> _repository;

    public DatabaseExchangeRateExporter(
        IRepository<ExchangeRateEntity> repository,
        ILogger<DatabaseExchangeRateExporter> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task ExportExchangeRatesAsync(IEnumerable<ExchangeRate> exchangeRates)
    {
        var entities = exchangeRates.Select(er => new ExchangeRateEntity
        {
            SourceCurrency = er.SourceCurrency.Code,
            TargetCurrency = er.TargetCurrency.Code,
            Rate = er.Value,
            Date = er.Date
        }).ToList();

        foreach (var entity in entities)
        {
            await _repository.AddExchangeRateAsync(entity);
            _logger.LogInformation("Added exchange rate: {Source} to {Target} = {Rate} on {Date}",
                entity.SourceCurrency, entity.TargetCurrency, entity.Rate, entity.Date);
        }

        await _repository.SaveChangesAsync();
        _logger.LogInformation("Successfully exported {Count} exchange rates to the database", entities.Count);
    }
}